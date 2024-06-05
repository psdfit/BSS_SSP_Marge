import { Component, OnInit, Input } from "@angular/core";
import { CommonSrvService } from "../../common-srv.service";
import { environment } from '../../../environments/environment';

@Component({
  selector: 'invoice-detail',
  templateUrl: './invoice-detail.component.html',
  styleUrls: ['./invoice-detail.component.scss']
})


export class InvoiceDetailComponent implements OnInit{

  constructor(private commonService: CommonSrvService) {
  }

  environment = environment;

  @Input() ID: any;

  INVOICE: any;
  
  getData() {
    this.commonService.getJSON("api/AdvancedSearch/GetInvoiceDetail?InvoiceID=" + this.ID).subscribe((data: any[]) => {
      this.INVOICE = data["Invoice"];
    });
  }

  ngOnInit(): void {
    this.getData();
  }
}
