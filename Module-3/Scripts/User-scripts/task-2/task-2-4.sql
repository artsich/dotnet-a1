--#1
select Suppliers.CompanyName from Northwind.Suppliers as Suppliers
where Suppliers.CompanyName in (
	select Suppliers.CompanyName from Northwind.Suppliers as Suppliers
	join Northwind.Products as Products on Products.SupplierID = Suppliers.SupplierID
	where Products.UnitsInStock = 0
)
order by CompanyName

--#2
select concat(Empl.FirstName, ' ', Empl.LastName) as 'Sellers', Empl_Amount.Amount 
from Northwind.Employees as Empl 
join (
	select EmployeeID, count(OrderID) as 'Amount' from Northwind.Orders
	group by EmployeeID
) as Empl_Amount
on Empl_Amount.EmployeeID = Empl.EmployeeID
where Empl_Amount.Amount > 150

--select EmployeeID, count(OrderID) as 'Amount' from Northwind.Orders
--group by EmployeeID
--having count(OrderId) > 150

--#3 exist not used here.
select Customers.*
from Northwind.Customers as Customers
left join Northwind.Orders as Orders
on Orders.CustomerID = Customers.CustomerID
where OrderID is null
