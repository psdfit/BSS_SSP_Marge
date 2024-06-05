export interface IOrgConfig {
  ConfigID: number;
  OID: number;
  OName: string;
  SchemeID: number;
  SchemeName: string;
  TSPMasterID: number;
  TSPName: string;
  ClassID: number;
  DualRegistration: boolean;
  BracketDaysBefore: number;
  BracketDaysAfter: number;
  EligibleGenderID: number;
  MinAge: number;
  MaxAge: number;
  MinEducation: number;
  ReportBracketBefore: number;
  ReportBracketAfter: number;
  StipendPayMethod: number;
  ClassStartFrom1: number;
  ClassStartTo1: number;
  ClassStartFrom2: number;
  ClassStartTo2: number;
  BISPIndexFrom: number;
  BISPIndexTo: number;
  MinAttendPercent: number;
  StipendDeductAmount: number;
  PhyCountDeductPercent: number;
  DeductDropOutPercent: number;
  StipNoteGenGTMonth: number;
  StipNoteGenLTMonth: number;
  MPRDenerationDay: number;
  InceptionReportFrom: number;
  InceptionReportTo: number;
  TraineesPerClassThershold: number;

}
