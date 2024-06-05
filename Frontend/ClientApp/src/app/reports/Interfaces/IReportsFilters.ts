export interface IProcurementFilters {
  SchemeID?: number;
  TSPID?: number;
  ClassID?: number;
  UserID?: number;
  ReportName?: string;
  ReportType?: string;
  DateMonth?: string;
  DownloadType?: string;
}
export interface IReports {
  ReportID?: number;
  ReportName?: string;
  SubReportID?: number;
  SubReportName?: string;
  FiltersID?: number[];
}

export interface IReportsFiltersName {
  Quarter?: [];
  ExaminationBody?: [];
  Trade?: [];
  Scheme?: [];
  Gender?: [];
  ProgramType?: [];
  Calendar?: [];
  Curriculum?: [];
  District?: [];
  EntryQualification?: [];
}
