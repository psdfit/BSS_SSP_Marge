
using System;

namespace DataLayer.Models
{
[Serializable]public class PBTEDataSharingTimelinesModel :ModelBase {
	public PBTEDataSharingTimelinesModel():base() { }
    public PBTEDataSharingTimelinesModel(bool InActive) : base(InActive) { }	public int ID	{ get; set; }	public int ClassTimeline	{ get; set; }	public int TSPTimeline	{ get; set; }	public int TraineeTimeline	{ get; set; }	public int DropOutTraineeTimeline	{ get; set; }}}
