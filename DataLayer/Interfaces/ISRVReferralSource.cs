using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Interfaces
{
    public interface ISRVReferralSource
    {
        List<ReferralSourceModel> FetchReferralSources(ReferralSourceModel model);
    }
}
