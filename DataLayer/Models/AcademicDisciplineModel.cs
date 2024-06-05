
using System;

namespace DataLayer.Models
{
[Serializable]public class AcademicDisciplineModel :ModelBase {
	public AcademicDisciplineModel():base() { }
    public AcademicDisciplineModel(bool InActive) : base(InActive) { }	public int AcademicDisciplineID	{ get; set; }	public string AcademicDisciplineName	{ get; set; }}}
