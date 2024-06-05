/* **** Aamer Rehman Malik *****/
import { Injectable } from '@angular/core';
import { ApprovalDialogueComponent } from '../custom-components/approval-dialogue/approval-dialogue.component';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { TStatusHistoryDialogueComponent } from '../custom-components/t-status-history-dialogue/t-status-history-dialogue.component';
import { TSPKAMHistoryDialogueComponent } from '../custom-components/tsp-kam-history-dialogue/tsp-kam-history-dialogue.component';
import { CRTraineeHistoryDialogueComponent } from '../custom-components/cr-trianee-history-dialogue/cr-trianee-history-dialogue.component';
import { TSPPendingClassesDialogueComponent } from '../custom-components/tsp-pending-classes-dialogue/tsp-pending-classes-dialogue.component';
import { DraftTraineeDialogueComponent } from '../custom-components/draft-trainee-dialogue/draft-trainee-dialogue.component';
import { KAMPendingClassesDialogueComponent } from '../custom-components/kam-pending-classes-dialogue/kam-pending-classes-dialogue.component';
import { KAMDeadlinesDialogComponent } from '../custom-components/kam-deadlines-dialog/kam-deadlines-dialog.component';
import { ExportConfirmDialogueComponent } from '../custom-components/export-confirm-dialogue/export-confirm-dialogue.component';
import { ExportExcel } from './Interfaces';
import { ClassMonthviewComponent } from '../custom-components/class-monthview/class-monthview.component';
import { DocumentDialogComponent } from '../custom-components/document-dialog/document-dialog.component';
import { ComplaintHistoryDialogueComponent } from 'src/app/complaint-module/complaint-history-dialogue/complaint-history-dialogue.component';
import { TSPColorChangedialogueComponent } from '../master-data/TSPColorChange-dialogue/TSPColorChange-dialogue.component';
import { TSPColorHistorydialogueComponent } from '../master-data/TSPColorHistory-dialogue/TSPColorHistory-dialogue.component';
import { NotificationDetaildialoguecomponent } from '../notification/NotificationDetail-dialogue/NotificationDetail-dialogue.component';
import { rolerightsdialogueComponent } from '../master-data/rolerights-dialogue/rolerights-dialogue.component';
import { TraineeJourneyComponent } from '../dashboard/trainee-journey/trainee-journey.component';
import { ClassJourneyComponent } from '../dashboard/class-journey/class-journey.component';

@Injectable({
  providedIn: 'root'
})
export class DialogueService {
  constructor(private dialog: MatDialog) { }

  public openApprovalDialogue(processKey: string, formID: number): Observable<boolean> {
    // let datas: IApprovalHistory = { ProcessKey: processKey, FormID: formID };
    const dialogRef = this.dialog.open(ApprovalDialogueComponent, {
      width: '60%',
      data: { ProcessKey: processKey, FormID: formID },
      disableClose: true,
    });
    return dialogRef.afterClosed();
  }
  public openTraineeStatusHistoryDialogue(traineeID: number): Observable<boolean> {
    const dialogRef = this.dialog.open(TStatusHistoryDialogueComponent, {
      width: '60%',
      data: { TraineeID: traineeID }
    });
    return dialogRef.afterClosed();
  }
  public openTSPKAMHistoryDialogue(tspID: number): Observable<boolean> {
    const dialogRef = this.dialog.open(TSPKAMHistoryDialogueComponent, {
      width: '60%',
      data: { TspID: tspID }
    });
    return dialogRef.afterClosed();
  }
  public openCrTraineeHistoryDialogue(traineeID: number): Observable<boolean> {
    const dialogRef = this.dialog.open(CRTraineeHistoryDialogueComponent, {
      width: '60%',
      data: { TraineeID: traineeID },
      disableClose: true,

    });
    return dialogRef.afterClosed();
  }
  public openTSPPendingClassesDialogue(tile: string): Observable<boolean> {
    const dialogRef = this.dialog.open(TSPPendingClassesDialogueComponent, {
      width: '60%',
      data: { TileName: tile },
      disableClose: true,

    });
    return dialogRef.afterClosed();
  }

  public OpenDraftTraineeDialogueComponent(tile: string): Observable<boolean> {
    const dialogRef = this.dialog.open(DraftTraineeDialogueComponent, {
      width: '60%',
      data: { TileName: tile },
      disableClose: true,

    });
    return dialogRef.afterClosed();
  }

  public openKAMPendingClassesDialogue(tile: string): Observable<boolean> {
    const dialogRef = this.dialog.open(KAMPendingClassesDialogueComponent, {
      width: '60%',
      data: { TileName: tile },
      disableClose: true,
    });
    return dialogRef.afterClosed();
  }
  public openKAMDeadlinesDialog(tile: string): Observable<boolean> {
    const dialogRef = this.dialog.open(KAMDeadlinesDialogComponent, {
      width: '60%',
      data: { TileName: tile },
      disableClose: true,

    });
    return dialogRef.afterClosed();
  }
  public openClassMonthviewDialogue(ClassID: number, Month: Date, type: string): Observable<boolean> {
    let typestr = '';
    if (type === 'PRN_C')
      typestr = 'Completion';
    else if (type === 'PRN_F')
      typestr = 'Employment';
    else if (type === 'INV_C')
      typestr = 'Completion';
    else if (type === 'INV_F')
      typestr = 'Employment';
    else
      typestr = 'Regular';
    const dialogRef = this.dialog.open(ClassMonthviewComponent, {
      // width: '80%',
      data: { ClassID, Month, Batch: typestr }
    });
    return dialogRef.afterClosed();
  }
  public openDocumentDialogue(ID: number, type: string): Observable<boolean> {

    const dialogRef = this.dialog.open(DocumentDialogComponent, {
      // width: '80%',
      data: { ID, Col: type }
    });
    return dialogRef.afterClosed();
  }
  public openMPRViewDialogue(MPRID: number): Observable<boolean> {
    const dialogRef = this.dialog.open(ClassMonthviewComponent, {
      // width: '80%',
      data: { MPRID }
    });
    return dialogRef.afterClosed();
  }
  public openExportConfirmDialogue(exportExcel: ExportExcel): Observable<boolean> {
    const dialogRef = this.dialog.open(ExportConfirmDialogueComponent, {
      width: '60%',
      data: exportExcel
    });
    return dialogRef.afterClosed();
  }
  public openComplaintHistoryDialogue(data: any): Observable<boolean> {
    const dialogRef = this.dialog.open(ComplaintHistoryDialogueComponent, {
      width: '62%',
      data: { data }
    });
    return dialogRef.afterClosed();
  }
  public openTSPColorChangeDialogue(data: any): Observable<boolean> {

    const dialogRef = this.dialog.open(TSPColorChangedialogueComponent, {
      width: '60%',
      data: { data }
    });
    return dialogRef.afterClosed();
  }
  public openTSPColorChangeHistoryDialogue(data: any): Observable<boolean> {

    const dialogRef = this.dialog.open(TSPColorHistorydialogueComponent, {
      width: '50%',
      data: { TSPMasterID: data.TSPMasterID }
    });
    return dialogRef.afterClosed();
  }
  public openDialogForNotificationDetail(data: any): Observable<boolean> {

    const dialogRef = this.dialog.open(NotificationDetaildialoguecomponent, {
      width: '60%',
      data: { data }
    });
    return dialogRef.afterClosed();
  }
  public openRoleRightsDialogue(data: any): Observable<boolean> {

    const dialogRef = this.dialog.open(rolerightsdialogueComponent, {
      width: '62%',
      data: { data }
    });
    return dialogRef.afterClosed();
  }
  public openTraineeJourneyDialogue(data: any): Observable<boolean> {

    const dialogRef = this.dialog.open(TraineeJourneyComponent, {
      width: '100%',
      data: { data }
    });
    return dialogRef.afterClosed();
  }
  public openClassJourneyDialogue(data: any): Observable<boolean> {

    const dialogRef = this.dialog.open(ClassJourneyComponent, {
      width: '100%',
      data: { data }
    });
    return dialogRef.afterClosed();
  }
}

