using DataLayer.Models;
using System.Collections.Generic;
using System.Data;

namespace DataLayer.Interfaces
{

    public interface ISRVBenchmarking
    {
        BenchmarkingModel GetByBenchmarkingID(int BenchmarkingID);
        List<BenchmarkingModel> SaveBenchmarking(BenchmarkingModel Benchmarking);
        List<BenchmarkingModel> FetchBenchmarking(BenchmarkingModel mod);
        List<BenchmarkingModel> FetchBenchmarking();
        List<BenchmarkingModel> FetchBenchmarking(bool InActive);
        void ActiveInActive(int BenchmarkingID, bool? InActive, int CurUserID);
        DataTable FetchBenchmarkingClasses(BenchmarkingModel mod);
    }

}
