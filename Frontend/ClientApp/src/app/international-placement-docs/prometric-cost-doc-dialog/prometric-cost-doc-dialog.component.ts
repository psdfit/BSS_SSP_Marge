import { Component, Inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, FormArray } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CommonSrvService } from "../../common-srv.service";
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { EnumApprovalProcess, EnumProgramCategory } from '../../shared/Enumerations';
import { EnumUserLevel } from "src/app/shared/Enumerations";
import { DialogueService } from "src/app/shared/dialogue.service";



@Component({
  selector: "app-prometric-cost-doc-dialog",
  templateUrl: "./prometric-cost-doc-dialog.component.html",
  styleUrls: ["./prometric-cost-doc-dialog.component.scss"],
})
export class PrometricCostDocDialogComponent implements OnInit {
  currentUser: any = {}
  uploadForm: FormGroup;
  tabIndex: number = 0; // Default to Upload tab
  isDragging: boolean = false; // Dragging state
  selectedFiles: File[] = []; // Initialize properly
  uploadedFiles: any[] = []; // Declare uploaded files array
  fetchedDocuments: any[] = []; // Store fetched documents
  selectedDocument: any = null; // Track the selected document for preview
  approvalStatus: string = '';
  rejectionReason: string = '';
  enumUserLevel = EnumUserLevel;
  isUploadDisabled: boolean = false;
  uploadMessage: string = "";


  private readonly UPLOAD_URL = "api/IPDocsVerification/UploadPrometricCostDocs"; // Replace with actual URL
  private readonly FETCH_DOCS_URL = "api/IPDocsVerification/GetPrometricCostDocs/"; // URL to fetch documents

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<PrometricCostDocDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private ComSrv: CommonSrvService,
    private sanitizer: DomSanitizer,
    private dialogue: DialogueService
  ) { }

  ngOnInit(): void {
    this.currentUser = this.ComSrv.getUserDetails();
    this.uploadForm = this.fb.group({
      traineeID: [this.data.traineeID],
      traineeName: [this.data.traineeName],
      traineeCode: [this.data.traineeCode],
      tsp: [this.data.tsp],
      tspID: [this.data.tspID],
      classCode: [this.data.classCode],
      files: this.fb.array([]), // Reactive form array for files
    });

    // Fetch documents when the component initializes
    this.fetchDocuments();

    // Call method to check document approval status
    this.setUploadRestrictions(this.data.PrometricCostStauts);

  }




  // Fetch documents from the backend
  fetchDocuments() {
    const traineeID = this.data.traineeID;
    this.ComSrv.getJSON(this.FETCH_DOCS_URL, traineeID).subscribe({
      next: (response: any) => {
        this.fetchedDocuments = response.map(doc => {
          const mimeType = this.getMimeTypeFromFileName(doc.FileName);
          const blob = this.decodeBase64ToBlob(doc.FileContentBase64, mimeType);
          const blobUrl = URL.createObjectURL(blob);

          return {
            ...doc,
            mimeType: mimeType,
            blobUrl: this.sanitizer.bypassSecurityTrustResourceUrl(blobUrl),
          };

        });



        // Auto-select the first document if available
        if (this.fetchedDocuments.length > 0) {
          this.selectedDocument = this.fetchedDocuments[0];

        }
      },
      error: (error) => {
        console.error("Error fetching documents", error);
      },
    });
  }


  setUploadRestrictions(selectedDocument: any) {
    if (selectedDocument === "Approved" || selectedDocument === "Rejected") {
      this.isUploadDisabled = true;
      this.uploadMessage = `You are not allowed to upload a new document as your document has already been ${selectedDocument}.`;
    } else {
      this.isUploadDisabled = false;
      this.uploadMessage = "";
    }
  }

  // Helper function to get MIME type from file name
  getMimeTypeFromFileName(fileName: string): string {
    const extension = fileName.split('.').pop()?.toLowerCase();
    switch (extension) {
      case 'pdf':
        return 'application/pdf';
      case 'jpg':
      case 'jpeg':
        return 'image/jpeg';
      case 'png':
        return 'image/png';
      // Add more cases as needed
      default:
        return 'application/octet-stream';
    }
  }

  // Open document preview
  openDocument(doc: any) {
    this.selectedDocument = doc;
    this.setUploadRestrictions(doc); // Check approval status
  }
  // Close document preview
  closePreview() {
    this.selectedDocument = null;
  }

  get filesArray(): FormArray {
    return this.uploadForm.get("files") as FormArray;
  }

  onFileSelected(event: any) {
    const newFile = event.target.files[0];
    if (!newFile) return; // If no file selected, exit
    // Check if file is a PDF
    if (newFile.type !== "application/pdf") {
      console.warn("Only PDF files are allowed.");
      return;
    }
    this.selectedFiles = [newFile]; // Replace array with only the new file
    this.filesArray.clear(); // Clear previous entries
    this.filesArray.push(this.fb.control(newFile)); // Add only the new file
  }

  removeFile(index: number) {
    this.selectedFiles.splice(index, 1); // Remove from selectedFiles array
    this.filesArray.removeAt(index);
  }

  uploadFiles() {
    if (this.uploadForm.invalid) {
      console.warn("Form is invalid", this.uploadForm.value);
      return;
    }

    const filePromises = this.selectedFiles.map(file => this.convertFileToBase64(file));
    Promise.all(filePromises).then(base64Files => {
      const requestData = {
        traineeID: this.uploadForm.value.traineeID,
        traineeName: this.uploadForm.value.traineeName,
        traineeCode: this.uploadForm.value.traineeCode,
        tspID: this.uploadForm.value.tspID,
        classCode: this.uploadForm.value.classCode,
        files: base64Files.map((base64, index) => ({
          fileName: this.selectedFiles[index].name,
          fileType: this.selectedFiles[index].type,
          fileContent: base64
        }))
      };

      this.ComSrv.postJSON(this.UPLOAD_URL, requestData).subscribe({
        next: response => {
          this.uploadedFiles = [...this.uploadedFiles, ...requestData.files];
          this.selectedFiles = [];
          this.filesArray.clear();
          this.fetchDocuments(); // Refresh the document list after upload
          // Show toast notification
          this.ComSrv.openSnackBar('Document uploaded successfully!', 'Close', 3000);
        },
        error: error => {
          console.error("Error uploading files", error);
          this.ComSrv.ShowError('Failed to upload document. Please try again.', 'Close', 3000);

        }
      });
    }).catch(error => {
      console.error("Error converting files to Base64", error);
    });
  }

  // Helper function to convert File to Base64
  convertFileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        const base64 = reader.result as string;
        // Remove the Data URL prefix
        const base64Content = base64.split(',')[1];
        resolve(base64Content);
      };
      reader.onerror = error => reject(error);
    });
  }

  // Helper function to convert Base64 to blob
  decodeBase64ToBlob(base64: string, mimeType: string): Blob {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: mimeType });
  }

  downloadDocument() {
    if (!this.selectedDocument) {
      console.warn("No document selected for download.");
      return;
    }
    // Convert safe URL to normal URL if necessary
    const blobUrl = this.selectedDocument.blobUrl.changingThisBreaksApplicationSecurity || this.selectedDocument.blobUrl;
    const link = document.createElement("a");
    link.href = blobUrl;
    link.download = this.selectedDocument.FileName; // Set the file name
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }

  submitApproval() {
    const requestData = {
      traineeID: this.uploadForm.value.traineeID,
      approvalStatus: this.approvalStatus,
      rejectionReason: this.approvalStatus === 'rejected' ? this.rejectionReason : null,
    };

    this.ComSrv.postJSON('api/IPDocsVerification/ApproveReject', requestData).subscribe({
      next: response => {
        console.log("Approval submitted successfully", response);
      },
      error: error => {
        console.error("Error submitting approval", error);
      }
    });
  }

  openApprovalDialogue(): void {

    let processKey = EnumApprovalProcess.IPPC;
    const traineeID = this.data.traineeID;
    this.ComSrv.getJSON(`api/IPDocsVerification/GetPrometricCostDocs/${traineeID}`).subscribe({
      next: (response: any) => {
        this.fetchedDocuments = response;

        if (this.fetchedDocuments.length > 0) {
          this.dialogue.openApprovalDialogue(processKey, this.fetchedDocuments[0].PrometricTraineeDocumentsID)
            .subscribe(result => {
              console.log(result);
              // location.reload();
            });
        } else {
          console.warn("No documents found.");
        }
      },
      error: (error) => {
        console.error("Error fetching documents", error);
      },
    });
  }

  // Drag and Drop Events
  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragLeave() {
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;
    if (event.dataTransfer?.files.length) {
      this.onFileSelected({ target: { files: event.dataTransfer.files } });
    }
  }
}