export class ModelBase {
  CreatedUserID: number;
  ModifiedUserID: number;
  CreatedDate: string;
  ModifiedDate: string;
  InActive: boolean;
}
export interface PagingModel {
  PageNo: number;
  PageSize: number;
  TotalCount: number;
  SortOrder: string;
  Sortdir: string;
  SearchColumn: string;
  SearchValue: string;
}
