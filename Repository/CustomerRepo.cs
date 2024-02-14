using API3.Models;
using API3.Repository.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace API3.Repository
{
    public class CustomerRepo : ICustomerRepo
    {


        readonly string connectionString = "";

        public CustomerRepo()
        {
            connectionString = "Data Source=APINP-ELPT9P8ER\\SQLEXPRESS;Initial Catalog=Northwind;User ID=tap2023;Password=tap2023;Encrypt=False";
        }


        public Customer GetCustomerById(string id)
        {
            Customer c = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
              string query = $@"
    SELECT 
        c.CustomerID, 
        c.CompanyName, 
        c.Address, 
        c.ContactName,
        o.OrderID, 
        o.OrderDate,
        o.ShipVia,
        s.CompanyName,
        s.Phone,
        o.ShipAddress,
        o.ShipName,
        od.ProductID,
        p.ProductName,
        od.Quantity,
        od.UnitPrice,
        od.Discount
    FROM 
        Customers c
    INNER JOIN
        Orders o ON c.CustomerID = o.CustomerID
    INNER JOIN
        Shippers s ON s.ShipperID = o.ShipVia
    INNER JOIN
        OrderDetails od ON o.OrderID = od.OrderID
    INNER JOIN
        Products p ON od.ProductID = p.ProductID
    WHERE 
        c.CustomerID = '{id}'";

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = query;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    c = new Customer();

                    c.CustomerId = dr["CustomerID"].ToString();
                    c.CompanyName = dr["CompanyName"].ToString();
                    c.ContactName = dr["ContactName"].ToString();


                }
            }
            return c;
        }


        public List<string> GetCustomers()
        {
            throw new NotImplementedException();
        }



        public Customer CreateCustomer(Customer customer)
        {
            string query = @"INSERT INTO Customers(CustomerID, CompanyName, ContactName) values(@CustomerID, @CompanyName, @ContactName)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                 
                    cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", customer.ContactName);
                 

                    con.Open();
                    cmd.ExecuteNonQuery(); 

                  

                    return customer; 
                }
            }
        }




        public Customer UpdateCustomer(Customer customer, string id)
        {
           
            string query = @"UPDATE Customers SET CompanyName = @CompanyName, ContactName = @ContactName where CustomerID=@CustomerID";




            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                  
                    cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@CompanyName", customer.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", customer.ContactName);
                 

                    con.Open();
                    cmd.ExecuteNonQuery(); 

                 

                    return customer; 
                }
            }
        }




        public void DeleteCustomer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Customer ID cannot be null or empty.");
            }


            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string deleteOrderDetailsQuery = "DELETE FROM [OrderDetails] WHERE OrderID IN (SELECT OrderID FROM Orders WHERE CustomerID = @CustomerID)";
                SqlCommand deleteOrderDetailsCmd = con.CreateCommand();
                deleteOrderDetailsCmd.CommandText = deleteOrderDetailsQuery;
                deleteOrderDetailsCmd.Parameters.AddWithValue("@CustomerID", id);
                con.Open();
                deleteOrderDetailsCmd.ExecuteNonQuery();

                string deleteOrdersQuery = "DELETE FROM Orders WHERE CustomerID = @CustomerID";
                SqlCommand deleteOrdersCmd = con.CreateCommand();
                deleteOrdersCmd.CommandText = deleteOrdersQuery;
                deleteOrdersCmd.Parameters.AddWithValue("@CustomerID", id);
                deleteOrdersCmd.ExecuteNonQuery();

                string deleteCustomerQuery = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand deleteCustomerCmd = con.CreateCommand();
                deleteCustomerCmd.CommandText = deleteCustomerQuery;
                deleteCustomerCmd.Parameters.AddWithValue("@CustomerID", id);
                deleteCustomerCmd.ExecuteNonQuery();
            }
        }

    }
}
