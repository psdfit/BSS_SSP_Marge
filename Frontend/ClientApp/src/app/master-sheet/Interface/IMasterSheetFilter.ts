export interface IMasterSheetFilter {
  SchemeID: number;
  ClassID: number;
  TSPID: number;
  UserID: number; 
  ClassStatusID: number; // For Class Status
  StartDate: string; // For Start Date
  EndDate: string; // For End Date
  FundingCategoryID: number; // For Project
  KamID: number;
}