/* **** Aamer Rehman Malik *****/
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  UsersAPIURL: "https://localhost:7046/",
  ReportsAPIURL: "https://localhost:44303/",
  //UsersAPIURL: "http://3.127.198.171:51599/",//"/core/",
  //ReportsAPIURL: "http://3.127.198.171:44303/",
  AuthToken: "UserModel",
  RightsToken: "RightsModel",
  FormIDRemoveForTSPUser: [28,77,1121,1125,1127,1139,1213],
  FormIDRemoveForAllUsers:[1139,1147],
  PrjTitle: "Punjab Skills Development Fund (PSDF)",
  PrjTitleShort: "PSDF",
  InActiveConfirmBoxTitle: "Active or Inactive this record",
  StatusConfirmBoxTitle: "Complaint status change this record",
  SaveMSG: "New ${Name} has been saved successfully",
  UpdateMSG: "${Name} has been Updated successfully",
  ActiveMSG: "${Name} enabled successfully",
  InActiveMSG: "${Name} disabled successfully",
  CancelMSG: "${Name} cancelled successfully",
  ActiveRTP: "${Name} created successfully",
  RemoveRTP: "${Name} removed successfully",
  CreateMSG: "${Name} created successfully",
  CustMSG: "${Name}",
  DateFormat: "dd/MM/yyyy",
  NewDateFormat: "dd/mm/yyyy",
  DateTimeFormat: "dd/MM/yyyy hh:mm:ss a",
  MonthFormat: "MM/yyyy",
  Decimal: "1.2-2",
  Mobile:"0000-0000000"

};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
