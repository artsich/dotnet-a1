--select * from Northwind.Customers as Customers

--#1
select Customers.CompanyName as c_name, Customers.Country as c_country from Northwind.Customers as Customers
where Country in('USA', 'Canada')
order by c_country;

--#2
select 
	Customers.CompanyName as c_name, 
	Customers.Country as c_country
from Northwind.Customers as Customers
where Country not in('USA', 'Canada')
order by c_name;

--#3
select distinct Customers.Country as 'Country' 
from Northwind.Customers as Customers
order by Customers.Country desc;