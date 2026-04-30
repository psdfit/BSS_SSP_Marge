import { Component, OnInit, OnDestroy, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { DomSanitizer, SafeResourceUrl } from "@angular/platform-browser";
import { CommonSrvService } from "src/app/common-srv.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { UsersModel } from "src/app/master-data/users/users.component";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

/**
 * TSP Invoice Submission Component
 *
 * Manages the invoice submission workflow for TSP (Training Service Provider) invoices.
 * Handles form validation, submission, file uploads, PRA registration, sales tax calculations,
 * and user notifications. Supports invoice generation, printing, and preview functionality.
 */
@Component({
  selector: "app-tsp-invoice-submission",
  templateUrl: "./tsp-invoice-submission.component.html",
  styleUrls: ["./tsp-invoice-submission.component.scss"],
})
export class TspInvoiceSubmissionComponent implements OnInit, OnDestroy {
  // ─── Invoice header fields ───────────────────────────────────────────────
  invoiceNo = "";
  supplierName = "";
  supplierNTN = "";
  supplierAddress = "";
  salesTaxRegNo = "";
  buyerName = "";
  buyerNTN = "";
  buyerAddress = "";
  invoiceDate = new Date();
  totalAmount = 0;
  isPRARegistered = false;
  salesTaxRate: number = 0;
  calculatedAmount = 0;
  salesTaxAmount = 0;
  amountInWords = "";

  // ─── Invoice lines ────────────────────────────────────────────────────────
  invoiceLines: any[] = [];
  invoiceHeader: any;
  displayedColumns: string[] = [
    "classCode",
    "description",
    "startDate",
    "endDate",
    "trainees",
    "netPayable",
  ];

  // ─── Subscription cleanup ─────────────────────────────────────────────────
  private destroy$ = new Subject<void>();

  // ─── File handling ────────────────────────────────────────────────────────
  selectedFile: File | null = null;
  uploadedFileName = "";
  private file: File | null = null;

  // ─── UI flags ─────────────────────────────────────────────────────────────
  isInvoiceGenerated = false;
  isSubmitted = false;
  readonly = false;

  // ─── Form group ───────────────────────────────────────────────────────────
  invoiceForm: FormGroup;

  // ─── Misc ─────────────────────────────────────────────────────────────────
  DataSource: SafeResourceUrl;
  currentUser: UsersModel;

  constructor(
    public dialogRef: MatDialogRef<TspInvoiceSubmissionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private sanitizer: DomSanitizer,
    private http: CommonSrvService,
    private fb: FormBuilder,
  ) {}

  // ─── Lifecycle ────────────────────────────────────────────────────────────

  ngOnInit(): void {
    this.currentUser = this.http.getUserDetails();
    this.initInvoiceForm();
    this.loadInvoiceData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ─── Form Setup ───────────────────────────────────────────────────────────

  initInvoiceForm(): void {
    this.invoiceForm = this.fb.group({
      TSPID: [""],
      InvoiceHeaderID: [""],
      IsPRARegistered: ["", Validators.required],
      SalesTaxRate: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
      InvoiceAttachment: ["", Validators.required],
    });
  }

  // ─── Data Loading ─────────────────────────────────────────────────────────

  loadInvoiceData(): void {
    this.invoiceHeader = this.data;
    this.invoiceNo = `${this.data.ProcessKey}_${this.data.InvoiceHeaderID}`;
    this.invoiceDate = this.data.U_Month ? new Date(this.data.U_Month) : new Date();

    this.invoiceForm.get("TSPID")?.setValue(this.data.TSPID);
    this.invoiceForm.get("InvoiceHeaderID")?.setValue(this.data.InvoiceHeaderID);

    this.getInvoiceBuyerSupplierInfo(this.data);
    this.getInvoiceLetterheadInfo(this.data);
    this.getInvoiceLines(this.data);
  }

  getInvoiceLines(r: any): void {
    if (r.InvoiceLines?.length > 0) {
      this.setInvoiceLines(r.InvoiceLines);
    } else {
      this.http
        .getJSON("api/Invoice/GetInvoiceLines/", r.InvoiceHeaderID)
        .pipe(takeUntil(this.destroy$))
        .subscribe((d: any) => this.setInvoiceLines(d));
    }
  }

  setInvoiceLines(lines: any[]): void {
    this.invoiceLines = lines.map((line: any) => ({
      ClassCode: line.ClassCode,
      Description: line.Description,
      SchemeName: line.SchemeName,
      TradeName: line.TradeName,
      StartDate: line.StartDate,
      EndDate: line.EndDate,
      Trainees: line.ClaimTrainees,
      NetPayableAmount: line.NetPayableAmount || 0,
      GrossPayable: line.GrossPayable || 0,
      LineTotal: line.LineTotal || 0,
      DropOutDeductionAmount: line.DropOutDeductionAmount || 0,
      AttendanceDeductionAmount: line.AttendanceDeductionAmount || 0,
      InvoiceType: line.InvoiceType,
    }));

    this.totalAmount = this.invoiceLines.reduce(
      (acc, item) => acc + (item.NetPayableAmount || 0),
      0,
    );

    this.calculateAmount();
    this.amountInWords = this.convertNumberToWords(this.calculatedAmount + this.salesTaxAmount);
  }

  getInvoiceBuyerSupplierInfo(r: any): void {
    this.http
      .getJSON("api/Invoice/GetInvoiceBuyerSupplierInfo/", r.TSPID)
      .pipe(takeUntil(this.destroy$))
      .subscribe((d: any) => {
        if (d?.length > 0) {
          this.buyerName = d[0].BuyerName || "";
          this.buyerNTN = d[0].BuyerNTN || "";
          this.buyerAddress = d[0].BuyerAddress || "";
          this.supplierName = d[0].SupplierName || "";
          this.supplierNTN = d[0].SupplierNTN || "";
          this.supplierAddress = d[0].SupplierAddress || "";
        }
      });
  }

  getInvoiceLetterheadInfo(r: any): void {
    this.http
      .getJSON("api/Invoice/GetInvoiceLetterheadInfo/", r.InvoiceHeaderID)
      .pipe(takeUntil(this.destroy$))
      .subscribe((response: any) => {
        if (!response?.length) return;

        const data = response[0];
// console.log("Loaded letterhead info:", data); 
        // Restore PRA registration status
        if (data.IsPRARegistered !== undefined && data.IsPRARegistered !== null) {
          const praValue = data.IsPRARegistered ? "yes" : "no";
          this.invoiceForm.get("IsPRARegistered")?.setValue(praValue);
          this.onPRARegisteredChange(praValue);
        }

        // Restore sales tax rate if present
        if (data.SalesTaxRate !== undefined && data.SalesTaxRate !== null) {
          this.salesTaxRate = parseFloat(data.SalesTaxRate) || 0;
          this.invoiceForm.get("SalesTaxRate")?.setValue(this.salesTaxRate);
          this.calculateAmount();
          this.amountInWords = this.convertNumberToWords(this.calculatedAmount + this.salesTaxAmount);
        }

        // Restore attachment
        if (data.InvoiceAttachment?.trim()) {
          this.invoiceForm.get("InvoiceAttachment")?.setValue(data.InvoiceAttachment);
          this.setExistingFileFromBase64(data.InvoiceAttachment);
        }

        // Restore letterhead info (kept for display, not upload)
        if (data.LetterheadFileName) {
          this.uploadedFileName = data.LetterheadFileName;
        }
      });
  }

  // ─── File Helpers ─────────────────────────────────────────────────────────

  /** Reconstruct a File object from saved Base64 data for display purposes */
  private setExistingFileFromBase64(base64Data: string): void {
    try {
      const mimeType = this.getMimeTypeFromBase64(base64Data);
      const extension = this.getExtensionFromMimeType(mimeType);
      const fileName = `invoice_${this.invoiceNo}.${extension}`;

      const base64Content = base64Data.split(",")[1];
      const byteString = atob(base64Content);
      const ab = new ArrayBuffer(byteString.length);
      const ia = new Uint8Array(ab);
      for (let i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
      }

      const blob = new Blob([ab], { type: mimeType });
      this.selectedFile = new File([blob], fileName, { type: mimeType });
      this.uploadedFileName = fileName;
    } catch {
      // silently ignore — file will just not show as pre-loaded
    }
  }

  private getMimeTypeFromBase64(base64Data: string): string {
    if (base64Data.startsWith("data:application/pdf")) return "application/pdf";
    if (base64Data.startsWith("data:image/jpeg")) return "image/jpeg";
    if (base64Data.startsWith("data:image/jpg")) return "image/jpeg";
    if (base64Data.startsWith("data:image/png")) return "image/png";
    return "application/octet-stream";
  }

  private getExtensionFromMimeType(mimeType: string): string {
    const map: Record<string, string> = {
      "application/pdf": "pdf",
      "image/jpeg": "jpg",
      "image/png": "png",
    };
    return map[mimeType] ?? "file";
  }

  validateFile(file: File): { valid: boolean; error?: string } {
    const maxSize = 5 * 1024 * 1024; // 5MB (matches template hint)
    const allowedTypes = ["application/pdf", "image/jpeg", "image/png"];

    if (!file) return { valid: false, error: "No file selected" };
    if (file.size > maxSize) return { valid: false, error: "File size must not exceed 5MB" };
    if (!allowedTypes.includes(file.type)) {
      return { valid: false, error: "Only PDF, JPG, and PNG files are allowed" };
    }
    return { valid: true };
  }

  convertFileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
    });
  }

  // ─── Tax / Amount Logic ───────────────────────────────────────────────────

  /**
   * Handle PRA registration toggle.
   * Enables/disables the SalesTaxRate control and recalculates totals.
   */
  onPRARegisteredChange(value: string): void {
    this.isPRARegistered = value === "yes";
    this.invoiceForm.get("IsPRARegistered")?.setValue(value);

    const rateCtrl = this.invoiceForm.get("SalesTaxRate");
    if (this.isPRARegistered) {
      rateCtrl?.setValidators([Validators.required, Validators.min(0), Validators.max(100)]);
      rateCtrl?.setValue(this.salesTaxRate);
      rateCtrl?.enable();
    } else {
      rateCtrl?.clearValidators();
      rateCtrl?.setValue(0);
      rateCtrl?.disable();
    }
    rateCtrl?.updateValueAndValidity();

    this.calculateAmount();
    this.amountInWords = this.convertNumberToWords(this.calculatedAmount + this.salesTaxAmount);
  }

  onSalesTaxRateChange(): void {
    const rateValue = this.invoiceForm.get("SalesTaxRate")?.value;
    if (rateValue !== null && rateValue !== undefined) {
      this.salesTaxRate = parseFloat(rateValue) || 0;
      this.calculateAmount();
      this.amountInWords = this.convertNumberToWords(this.calculatedAmount + this.salesTaxAmount);
    }
  }

  /**
   * Calculates tax-exclusive and tax amounts.
   * Formula: tax = (total × rate) / (100 + rate)  — i.e. tax is already included in totalAmount.
   */
  calculateAmount(): void {
    if (this.isPRARegistered && this.salesTaxRate > 0) {
      this.salesTaxAmount = (this.totalAmount * this.salesTaxRate) / (100 + this.salesTaxRate);
      this.calculatedAmount = this.totalAmount - this.salesTaxAmount;
    } else {
      this.salesTaxAmount = 0;
      this.calculatedAmount = this.totalAmount;
    }
  }

  // ─── Actions ──────────────────────────────────────────────────────────────

  generateInvoice(): void {
    if (!this.invoiceForm.get("IsPRARegistered")?.value) {
      this.http.ShowError("Please select whether PRA is registered or not");
      return;
    }
    this.isInvoiceGenerated = true;
    this.http.openSnackBar("Invoice generated successfully!");
  }

  /**
   * Opens a styled print window with the invoice content.
   */
  printInvoice(): void {
    if (!this.invoiceForm.get("IsPRARegistered")?.value) {
      this.http.ShowError("Please confirm whether you are registered with PRA or not.");
      return;
    }

    const printContent = document.getElementById("invoicePrintArea");
    if (printContent) {
      const printWindow = window.open("", "_blank");
      if (printWindow) {
        printWindow.document.write(this.getPrintHtml());
        printWindow.document.close();
        printWindow.print();
      }
    }
  }

  private getPrintHtml(): string {
    const date = this.invoiceDate;
    const formattedDate = `${date.getFullYear()}_${String(date.getMonth() + 1).padStart(2, "0")}_${String(date.getDate()).padStart(2, "0")}`;

    return `
      <html>
        <head>
          <title>${this.invoiceNo}_${this.supplierName}_${formattedDate}</title>
          <style>
            * { margin: 0; padding: 0; box-sizing: border-box; }
            html, body { height: 100%; }
            body { font-family: Arial, sans-serif; font-size: 12px; line-height: 1.5; display: flex; justify-content: center; align-items: center; min-height: 100vh; background: #fff; }
            .invoice-container { width: 800px; padding: 48px; background: #fff; }
            .invoice-header { text-align: center; margin-bottom: 30px; }
            .invoice-title { font-size: 22px; font-weight: bold; margin-bottom: 6px; text-transform: uppercase; letter-spacing: 1px; }
            .invoice-date { font-size: 12px; color: #555; margin-top: 4px; }
            .party-details { width: 100%; display: flex; justify-content: space-between; border: 1px solid #333; padding: 15px; margin-bottom: 20px; }
            .supplier-details, .buyer-details { width: 48%; }
            .party-title { font-weight: bold; font-size: 13px; margin-bottom: 10px; border-bottom: 1px solid black; padding-bottom: 5px; }
            .party-info { font-size: 11px; line-height: 1.8; }
            .amount-section { width: 100%; margin-top: 20px; }
            .amount-row { display: flex; justify-content: flex-end; padding: 5px 0; }
            .amount-label { width: 260px; font-weight: bold; }
            .amount-value { width: 160px; text-align: right; }
            .total-row { font-size: 14px; font-weight: bold; background: #f5f5f5; padding: 8px 0; }
            .amount-in-words { margin-top: 15px; font-style: italic; }
            @media print {
              @page { margin: 0; size: A4; }
              html, body { height: 100%; width: 100%; display: flex; justify-content: center; align-items: center; margin: 0; padding: 0; }
              .invoice-container { width: 720px; padding: 40px; margin: auto; }
            }
          </style>
        </head>
        <body>
          <div class="invoice-container">
            <div class="invoice-header">
              <div class="invoice-title">${this.isPRARegistered ? "Punjab Sales Tax Invoice" : "Invoice"}</div>
              <div class="invoice-date">Invoice Date: ${this.formatFullDate(new Date())}</div>
            </div>
            <div class="party-details">
              <div class="supplier-details">
                <div class="party-title">Invoice No: ${this.invoiceNo}</div>
                <div class="party-title">Supplier Details</div>
                <div class="party-info">
                  <strong>Name:</strong> ${this.supplierName || "N/A"}<br>
                  <strong>NTN:</strong> ${this.supplierNTN || "N/A"}<br>
                  <strong>Address:</strong> ${this.supplierAddress || "N/A"}<br>
                  ${this.salesTaxRegNo ? `<strong>STRN:</strong> ${this.salesTaxRegNo}<br>` : ""}
                </div>
              </div>
              <div class="buyer-details">
                <div class="party-title">Invoice Month: ${this.formatDate(this.invoiceDate)}</div>
                <div class="party-title">Buyer Details</div>
                <div class="party-info">
                  <strong>Name:</strong> ${this.buyerName || "N/A"}<br>
                  <strong>NTN:</strong> ${this.buyerNTN || "N/A"}<br>
                  <strong>Address:</strong> ${this.buyerAddress || "N/A"}
                </div>
              </div>
            </div>
            <div class="amount-section">
              <div class="amount-row">
                <span class="amount-label">Amount Excluding Sales Tax (Rs.):</span>
                <span class="amount-value">${this.formatCurrency(this.totalAmount - this.salesTaxAmount)}</span>
              </div>
              <div class="amount-row">
                <span class="amount-label">PST%:</span>
                <span class="amount-value">${this.isPRARegistered ? this.salesTaxRate : 0}%</span>
              </div>
              <div class="amount-row">
                <span class="amount-label">PST (Rs.):</span>
                <span class="amount-value">${this.formatCurrency(this.salesTaxAmount)}</span>
              </div>
              <div class="amount-row total-row">
                <span class="amount-label">Total Amount (Rs.):</span>
                <span class="amount-value">${this.formatCurrency(this.calculatedAmount + this.salesTaxAmount)}</span>
              </div>
              <div class="amount-in-words">
                <strong>Amount In Words:</strong> ${this.amountInWords}
              </div>
            </div>
          </div>
        </body>
      </html>
    `;
  }

  // ─── File Events ──────────────────────────────────────────────────────────

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (!file) return;

    const validation = this.validateFile(file);
    if (!validation.valid) {
      this.http.ShowError(validation.error);
      event.target.value = "";
      return;
    }

    this.file = file;
    this.selectedFile = file;
    this.uploadedFileName = file.name;

    this.convertFileToBase64(file)
      .then((base64) => this.invoiceForm.get("InvoiceAttachment")?.setValue(base64))
      .catch(() => this.http.ShowError("Failed to read file"));
  }

  openAttachmentPreview(): void {
    const dataUrl = this.invoiceForm.get("InvoiceAttachment")?.value;
    if (!dataUrl) {
      this.http.ShowError("No file selected to preview");
      return;
    }

    const previewWindow = window.open("", "_blank");
    if (!previewWindow) {
      this.http.ShowError("Unable to open preview window");
      return;
    }

    const isPdf = dataUrl.startsWith("data:application/pdf");
    const isImage = dataUrl.startsWith("data:image/");

    let html = '<html><head><title>Attachment Preview</title></head><body style="margin:0;">';
    if (isPdf) {
      html += `<embed src="${dataUrl}" type="application/pdf" width="100%" height="100%" />`;
    } else if (isImage) {
      html += `<img src="${dataUrl}" style="max-width:100%;height:auto;display:block;margin:auto;" />`;
    } else {
      html += `<p style="padding:1rem;">Preview not supported for this file type.</p>`;
    }
    html += "</body></html>";

    previewWindow.document.write(html);
    previewWindow.document.close();
  }

  clearSelectedFile(): void {
    this.selectedFile = null;
    this.file = null;
    this.uploadedFileName = "";
    this.invoiceForm.get("InvoiceAttachment")?.setValue("");
    const fileInput = document.querySelector("#fileInput") as HTMLInputElement;
    if (fileInput) fileInput.value = "";
  }

  // ─── Submit ───────────────────────────────────────────────────────────────

  submitInvoice(): void {
    if (!this.invoiceForm) return;

    const formValue = this.invoiceForm.value;

    if (!formValue.IsPRARegistered) {
      this.http.ShowError("Please select whether PRA is registered or not");
      return;
    }

    if (!formValue.InvoiceAttachment?.trim()) {
      this.http.ShowError("Please select the invoice file");
      return;
    }

    if (this.file) {
      const validation = this.validateFile(this.file);
      if (!validation.valid) {
        this.http.ShowError(validation.error);
        return;
      }
    }

    if (this.invoiceForm.invalid) {
      this.http.ShowError("Please fill all required fields");
      return;
    }

    const payload = {
      TSPID: formValue.TSPID,
      InvoiceHeaderID: formValue.InvoiceHeaderID,
      IsPRARegistered: this.convertToBoolean(formValue.IsPRARegistered),
      SalesTaxRate: this.salesTaxRate,
      InvoiceAttachment: formValue.InvoiceAttachment,
    };

    this.http
      .postJSON("api/Invoice/SaveInvoiceLetterhead", payload)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.http.openSnackBar("Invoice submitted successfully");
          this.dialogRef.close(true);
        },
        error: () => {
          this.http.ShowError("Submission failed. Please try again.");
        },
      });
  }

  // ─── Utilities ────────────────────────────────────────────────────────────

  private convertToBoolean(value: any): boolean {
    return value === true || value === "true" || value === "yes";
  }

  private normalizeAmount(amount: number): number {
    return parseFloat(amount.toFixed(2));
  }

  formatDate(date: Date): string {
    if (!date) return "";
    return new Date(date).toLocaleDateString("en-PK", { year: "numeric", month: "long" });
  }

  formatFullDate(date: Date): string {
    if (!date) return "";
    return new Date(date).toLocaleDateString("en-PK", { year: "numeric", month: "long", day: "numeric" });
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat("en-PK", { style: "currency", currency: "PKR" })
      .format(this.normalizeAmount(amount));
  }

  convertNumberToWords(num: number): string {
    num = this.normalizeAmount(num);

    const ones = ["", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
      "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
      "Seventeen", "Eighteen", "Nineteen"];
    const tens = ["", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"];
    const scales = ["", "Thousand", "Million", "Billion"];

    const convertHundreds = (n: number): string => {
      let str = "";
      if (n > 99) {
        str += ones[Math.floor(n / 100)] + " Hundred";
        n %= 100;
        if (n) str += " ";
      }
      if (n > 19) {
        str += tens[Math.floor(n / 10)];
        if (n % 10) str += " " + ones[n % 10];
      } else if (n > 0) {
        str += ones[n];
      }
      return str;
    };

    if (num === 0) return "Rupees Zero Only";

    let words = "";
    let scaleIndex = 0;
    let n = Math.floor(num);

    while (n > 0) {
      const chunk = n % 1000;
      if (chunk) {
        const chunkWords = convertHundreds(chunk);
        words = chunkWords + (scales[scaleIndex] ? " " + scales[scaleIndex] : "") + " " + words;
      }
      n = Math.floor(n / 1000);
      scaleIndex++;
    }

    return "Rupees " + words.trim() + " Only";
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }
}