
using System;

namespace DataLayer.Models
{
[Serializable]public class SourceOfCurriculumModel :ModelBase {
	public SourceOfCurriculumModel():base() { }
    public SourceOfCurriculumModel(bool InActive) : base(InActive) { }	public int SourceOfCurriculumID	{ get; set; }	public string Name	{ get; set; }
    //public int CertAuthID { get; set; }
    //public string CertAuthName { get; set; }

    }}
