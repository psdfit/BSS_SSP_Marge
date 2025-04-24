using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using DataLayer.Classes;
using DataLayer.Models;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using System.IO;
namespace DataLayer.Services
{
    public class SRVMobileAppDownload : ISRVMobileAppDownload
    {
        private readonly string filePath = @"C:\Users\umair.nadeem\source\repos\BSS_SSP_Marge\PSDF_BSS\Assets\MobileApp\myApp.zip";

        public byte[] GetMobileApp()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.");

            return File.ReadAllBytes(filePath);
        }
    }
}
