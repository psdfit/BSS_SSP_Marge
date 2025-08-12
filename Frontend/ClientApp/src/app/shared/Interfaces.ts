export interface SearchFilter {
  SchemeID?: number;
  TSPID?: number;
  ClassID?: number;
  Schemes?: string;
  TSPs?: string;
  Classes?: string;
  TraineeID?: number;
  OID?: number;
  SelectedColumns?: string[]
  ClassStatusID?: number, // Class Status
  StartDate?: string, // Start Date
  EndDate?: string, // End Date
  FundingCategoryID?: number, // Project 
  KamID?: number // KAM
}

export interface SearchFilterMultiSelect {
  SchemeID?: [];
  TSPID?: [];
  ClassID?: [];
  Schemes?: [];
  TSPs?: [];
  Classes?: [];
  TraineeID?: number;
  OID?: number;
  SelectedColumns?: string[]
  ClassStatusID?: number, // Class Status
  StartDate?: string, // Start Date
  EndDate?: string, // End Date
  FundingCategoryID?: number, // Project 
  KamID?: number // KAM
}




export interface SearchFilterTAR {
  SchemeID: number;
  TSPID: number;
  ClassID: number;
  TraineeID: number;
  OID: number;
  SelectedColumns: any[]; // Adjust the type as needed
  Month: number | null;   // Add Month property
  Year: number | null;    // Add Year property
}
export interface ExportExcel {
  Title: string;
  Author: string;
  Type: number;
  Data?: any;
  List1: any[];
  List2?: any[];
  ImageFieldNames?: string[];
  SearchFilters?: SearchFilter | SearchFilterMultiSelect;
  // SearchFilterMultiSelect?: SearchFilterMultiSelect;
  DataModel?: object;
  LoadDataAsync?: any;
  Month?: Date;

}
export interface PagingModel {
  PageNo: number;
  PageSize: number;
  TotalCount?: number;
  SortColumn: string;
  SortOrder: string;
  SearchColumn: string;
  SearchValue: string;

}
