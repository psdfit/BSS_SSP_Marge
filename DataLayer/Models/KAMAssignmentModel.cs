
using System;

namespace DataLayer.Models
{
[Serializable]public class KAMAssignmentModel :ModelBase {
	public KAMAssignmentModel():base() { }
    public KAMAssignmentModel(bool InActive) : base(InActive) { }	public int KamID	{ get; set; }	public int AssignedUser { get; set; }	public int UserID	{ get; set; }	public string UserName { get; set; }	public string Email { get; set; }	public string ContactNo { get; set; }	public string FullName { get; set; }	public int TspID	{ get; set; }	public string TSPName { get; set; }	public string DistrictName { get; set; }	public string RegionName { get; set; }	public int DistrictID	{ get; set; }	public int RegionID	{ get; set; }	public int ClassID	{ get; set; }	public int TSPMasterID	{ get; set; }	public int RoleID	{ get; set; }        

    }}
