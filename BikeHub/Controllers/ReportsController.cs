using BikeHub.Models;
using BikeHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

/*
 * Reports Controller:
 * Generate Reports according to a spesific filters 
 * @Author:Rawah Almesri
 * Date: Nov 14,2024
 */


namespace BikeHub.Controllers
{
    public class ReportsController : Controller
    {
        private readonly RentalRepository _rentalRepository;

        public ReportsController(RentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public IActionResult GenerateReport()
        {
            return View(); //  filter form
        }

        [HttpPost]
        public async Task<IActionResult> GenerateReport(
            DateTime? startDate,
            DateTime? endDate,
            string? customerName,
            string? bikeRented,
            string? availabilityStatus,
            bool? paid,
            bool? overdue,
            string exportType)
        {
            var rentals = await _rentalRepository.GetFilteredRentalsAsync(
                startDate,
                endDate,
                customerName,
                bikeRented,
                availabilityStatus,
                paid,
                overdue);

            if (exportType == "Excel")
            {
                var file = ExportToExcel(rentals);
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RentalReport.xlsx");
            }

            return View("ReportResults", rentals); // Display results in the view
        }

        private byte[] ExportToExcel(List<Rental> rentals)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Rental Report");

                // Add headers
                worksheet.Cells[1, 1].Value = "Rental ID";
                worksheet.Cells[1, 2].Value = "Customer Name";
                worksheet.Cells[1, 3].Value = "Bike Rented";
                worksheet.Cells[1, 4].Value = "Date Rented";
                worksheet.Cells[1, 5].Value = "Due Date";
                worksheet.Cells[1, 6].Value = "Paid";
                worksheet.Cells[1, 7].Value = "Availability Status";

                // Add data cells
                for (int i = 0; i < rentals.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = rentals[i].Id;
                    worksheet.Cells[i + 2, 2].Value = rentals[i].Name;
                    worksheet.Cells[i + 2, 3].Value = rentals[i].BikeRented;
                    worksheet.Cells[i + 2, 4].Value = rentals[i].DateRented.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 5].Value = rentals[i].DueDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 6].Value = rentals[i].Paid ? "Yes" : "No";
                    worksheet.Cells[i + 2, 7].Value = rentals[i].AvailabilityStatus;
                }

                return package.GetAsByteArray();
            }

        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

