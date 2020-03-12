--#1
select round(sum(Details.UnitPrice * Details.Quantity * (1 - Details.Discount)), 2) as Total from Northwind.[Order Details] as Details

--#2
select count(Orders.ShippedDate) as 'Count' from Northwind.Orders as Orders

--#3
select count(distinct Orders.CustomerID) from Northwind.Orders as Orders

