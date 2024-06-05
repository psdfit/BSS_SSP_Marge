
using System;

namespace DataLayer.Models
{
    [Serializable]

    public class DeoDashboardModel : ModelBase
    {
        public DeoDashboardModel()  { }
        public int PendingEmploymentVerifications { get; set; }
        public int PendingCNICVerifications { get; set; }

    }}
