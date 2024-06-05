
using System;

namespace DataLayer.Models
{
[Serializable]public class TraineeAttendanceModel :ModelBase {
	public TraineeAttendanceModel():base() { }
    public TraineeAttendanceModel(bool InActive) : base(InActive) { }	public int TraineeAttendanceID	{ get; set; }	public int TraineeProfileID	{ get; set; }	public bool IsManual	{ get; set; }	public string GeoLocation	{ get; set; }	public DateTime AttendanceDate	{ get; set; }	public bool IsAbsent	{ get; set; }}}
