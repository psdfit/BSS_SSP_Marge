
using System;

namespace DataLayer.Models
{
[Serializable]
	public ClassStatusModel():base() { }
    public ClassStatusModel(bool InActive) : base(InActive) { }
	public string ClassReason { get; set; }


	}