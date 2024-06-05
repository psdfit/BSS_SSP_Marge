using System;
using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DataLayer.Classes;
using DataLayer.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System.Data;

namespace PSDF_BSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ISRVReports srvReports;
        private readonly ISRVKAMAssignment ISRVKAMAssignment;

        public ReportsController(ISRVKAMAssignment ISRVKAMAssignment, ISRVReports srvReports)
        {
            this.srvReports = srvReports;
            this.ISRVKAMAssignment = ISRVKAMAssignment;
        }
        [HttpGet]
        [Route("RD_Reports")]
        public IActionResult RD_Reports()
        {
            try
            {
                return Ok(srvReports.FetchReports());
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
        [HttpGet]
        [Route("RD_Reports/{RoleID}")]
        public IActionResult RD_ReportsByRoleID(int RoleID)
        {
            try
            {

                if (RoleID == 1)
                {
                    RoleID = 0;
                }
                return Ok(srvReports.FetchReportsByRoleID(RoleID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
        [HttpGet]
        [Route("RD_SubReports")]
        public IActionResult RD_SubReports(int? ReportID)
        {
            try
            {
                return Ok(srvReports.FetchSubReports(ReportID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
        [HttpGet]
        [Route("RD_SubReportsFilters")]
        public IActionResult RD_SubReportsFilters(int? SubReportID)
        {
            try
            {
                return Ok(srvReports.FetchSubReportsFilters(SubReportID));
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
        [HttpGet]
        [Route("GetFilterData")]
        public IActionResult GetFilterData(string FilterName, int? UserID)
        {
            try
            {
                object obj = new object();
                if (FilterName == "Quarter")
                {
                    List<FilterModel> quarter = new List<FilterModel>
                    {
                        new FilterModel( 1,"Q1"),
                        new FilterModel( 2,"Q2"),
                        new FilterModel( 3,"Q3"),
                        new FilterModel( 4,"Q4")
                    };
                    obj = quarter;
                }
                if (FilterName == "ExaminationBody")
                {
                    FilterName = "Examination Body";
                    obj = new SRVCertificationAuthority().FetchCertificationAuthority(false).Select(x => new
                    {
                        ID = x.CertAuthID,
                        Name = x.CertAuthName,
                    });
                }
                if (FilterName == "Sector")
                {
                    obj = new SRVSector().FetchSector(false).Select(x => new
                    {
                        ID = x.SectorID,
                        Name = x.SectorName,
                    });
                }
                if (FilterName == "Duration")
                {
                    obj = new SRVDuration().FetchDuration().Select(x => new
                    {
                        ID = x.DurationID,
                        Name = x.Duration.ToString(),
                    });
                }
                if (FilterName == "Trade")
                {
                    obj = new SRVDashboard().FetchTrades().AsEnumerable().Select(x => new
                    {
                        ID = x.Field<int>("TradeID"),
                        Name = x.Field<string>("TradeName"),
                    });
                }
                if (FilterName == "Scheme")
                {
                    obj = new SRVDashboard().FetchSchemes().AsEnumerable().Select(x => new
                    {
                        ID = x.Field<int>("SchemeID"),
                        Name = x.Field<string>("SchemeName"),
                    });
                }
                if (FilterName == "Gender")
                {
                    obj = new SRVGender().FetchGender(false).Select(x => new
                    {
                        ID = x.GenderID,
                        Name = x.GenderName,
                    });
                }
                if (FilterName == "ProgramType")
                {
                    FilterName = "Program Type";
                    obj = new SRVProgramType().FetchProgramType(false).Select(x => new
                    {
                        ID = x.PTypeID,
                        Name = x.PTypeName,
                    });
                }
                if (FilterName == "Calendar")
                    obj = (1);
                if (FilterName == "StartDate")
                {
                    FilterName = "Start Date";
                    obj = (1); 
                }
                if (FilterName == "EndDate")
                {
                    FilterName = "End Date";
                    obj = (1);
                }
                if (FilterName == "Cluster")
                {
                    obj = new SRVDashboard().FetchClusters().AsEnumerable().Select(x => new
                    {
                        ID = x.Field<int>("ClusterID"),
                        Name = x.Field<string>("ClusterName"),
                    });
                }
                if (FilterName == "Curriculum")
                {
                    obj = new SRVSourceOfCurriculum().FetchSourceOfCurriculum(false).Select(x => new
                    {
                        ID = x.SourceOfCurriculumID,
                        Name = x.Name,
                    });
                }
                if (FilterName == "District")
                {
                    obj = new SRVDashboard().FetchDistricts().AsEnumerable().Select(x => new
                    {
                        ID = x.Field<int>("DistrictID"),
                        Name = x.Field<string>("DistrictName"),
                    });
                }
                if (FilterName == "EntryQualification")
                {
                    FilterName = "Entry Qualification";
                    obj = new SRVEducationTypes().FetchEducationTypes(false).Select(x => new
                    {
                        ID = x.EducationTypeID,
                        Name = x.Education,
                    });
                }
                if (FilterName == "FinancialYear")
                {
                    FilterName = "Financial Year";
                    List<FilterModel> fy = new List<FilterModel>
                    {
                        new FilterModel( 1,"2020-2021"),
                    };
                    obj = fy;
                }
               if (FilterName == "Tsp")
                {
                    obj = new SRVDashboard().FetchTSPDetail().AsEnumerable().Select(x => new
                    {
                        ID = x.Field<int>("TSPID"),
                        Name = x.Field<string>("TSPName"),
                    });
                }
                if (FilterName == "Class")
                {
                    obj = new SRVClass().FetchClassesDataByUser(UserID.Value).Select(x => new
                    {
                        ID = x.ClassID,
                        Name = x.ClassCode,
                    });
                }
                if (FilterName == "User")
                {
                    FilterName = "KAM";
                    obj = ISRVKAMAssignment.FetchKAMAssignmentForFilters(false).Select(x => new
                    {
                        ID = x.UserID,
                        Name = x.UserName,
                    });
                }
                if (FilterName == "Funding")
                {
                    FilterName = "Project Type";
                    obj = new SRVFundingSource().FetchFundingSource(false).Select(x => new
                    {
                        ID = x.FundingSourceID,
                        Name = x.FundingSourceName,
                    });
                }
                if(FilterName == "EmploymentStatus")
                {
                    FilterName = "Employment Status";
                    List<FilterModel> es = new List<FilterModel>
                    {
                        new FilterModel( 1,"Yes"),
                        new FilterModel( 2,"No"),
                    };
                    obj = es;
                }
                if(FilterName == "ProgramCategory")
                {
                    FilterName = "Program Category";
                    obj = new SRVProgramCategory().FetchProgramCategory(false).Select(x => new
                    {
                        ID = x.PCategoryID,
                        Name = x.PCategoryName,
                    });
                }
                Pair<string, object> pair = new Pair<string, object>(FilterName, obj);
                return Ok(pair);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
        [HttpGet]
        [Route("RD_SchemeByProgramCategory")]
        public IActionResult SchemeByByProgramCategory(int ID)
        {
            try
            {
                object obj = new SRVDashboard().FetchSchemesByProgramCategory(ID).AsEnumerable().Select(x => new
                {
                    ID = x.Field<int>("SchemeID"),
                    Name = x.Field<string>("SchemeName"),
                });
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
        [HttpGet]
        [Route("RD_TSPByScheme")]
        public IActionResult TSPByScheme(int ID)
        {
            try
            {
                object obj = new SRVDashboard().FetchTSPsByScheme(ID).AsEnumerable().Select(x => new
                {
                    ID = x.Field<int>("TSPID"),
                    Name = x.Field<string>("TSPName"),
                });
                return Ok(obj);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
    }
    class FilterModel : ModelBase
    {
        public FilterModel() : base()
        {
        }
        public FilterModel(int ID, string Name) : base()
        {
            this.ID = ID;
            this.Name = Name;
        }
        public String Name { get; set; }
        public int ID { get; set; }
    }
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    }
}