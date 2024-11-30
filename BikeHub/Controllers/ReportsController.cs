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
        private readonly CustomerRepository _customerRepository;

        public ReportsController(RentalRepository rentalRepository, CustomerRepository customerRepository)
        {
            _rentalRepository = rentalRepository;
            _customerRepository=customerRepository;
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
        [HttpPost]
        public async Task<IActionResult> CustomerReport(string? fname, string? lname, string? email, string? campus, string? typeofcus, string exportType)
        {
            var filteredCustomers = await _customerRepository.GetFilteredCustomersAsync(
                fname, lname, email, campus, typeofcus);

            if (exportType == "Excel")
            {
                var file = ExportCustomersToExcel(filteredCustomers);
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerReport.xlsx");
            }

            // Pass the filtered data to the view if no export
            return View("CustomerReport", filteredCustomers);
        }

        private byte[] ExportCustomersToExcel(List<Customer> customers)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Customer Report");

                // Add headers
                worksheet.Cells[1, 1].Value = "Customer ID";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Last Name";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Campus";
                worksheet.Cells[1, 6].Value = "Type of Customer";

                // Add data cells
                for (int i = 0; i < customers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = customers[i].StudentId;
                    worksheet.Cells[i + 2, 2].Value = customers[i].FirstName;
                    worksheet.Cells[i + 2, 3].Value = customers[i].LastName;
                    worksheet.Cells[i + 2, 4].Value = customers[i].Email;
                    worksheet.Cells[i + 2, 5].Value = customers[i].CampusName;
                    worksheet.Cells[i + 2, 6].Value = customers[i].TypeOfCustomer;
                }

                // Return the Excel file as a byte array
                return package.GetAsByteArray();
            }
        }



        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CustomerReport()
        {
            return View();
        }

    }
}

