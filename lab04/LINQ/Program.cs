using System;
using System.ComponentModel;

namespace linq;

class wczytywacz<T>{
    public List<T> wczytajListe(String path, Func<String[], T> generuj){
        List<T> elements = new List<T>();        
        foreach(var line in File.ReadAllLines(path)){
            string[] data = line.Split(',');
            elements.Add(generuj(data));
        }
        elements.RemoveAt(0); //usuwamy header
        return elements;
    }
}

class Program{
    static void Main(){

        // Wczytanie wszystkich elementów do lsit

        wczytywacz<Region> r = new wczytywacz<Region>();
        List<Region> regionsList = r.wczytajListe("./resources/regions.csv",
            x => new Region(x[0], x[1]));

        wczytywacz<Territory> t = new wczytywacz<Territory>();
        List<Territory> territoriesList = t.wczytajListe("./resources/territories.csv",
            x => new Territory(x[0], x[1], x[2]));

        wczytywacz<EmployeeTerritories> et = new wczytywacz<EmployeeTerritories>();
        List<EmployeeTerritories> employeeTerritoriesList = et.wczytajListe("./resources/employee_territories.csv",
            x => new EmployeeTerritories(x[0], x[1]));

        wczytywacz<Employee> e = new wczytywacz<Employee>();
        List<Employee> employeesList = e.wczytajListe("./resources/employees.csv",
            x => new Employee(x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7], x[8], x[9],
                                x[10], x[11], x[12], x[13], x[14], x[15], x[16], x[17]));

        // Zad 1 - wybierz nazwiska pracowników
        var result1 = employeesList.Select(e => e.LastName);

        foreach(var lname in result1){
            System.Console.WriteLine(lname);
        }

        /* Zad 2 - wypisz nazwiska pracowników oraz dla każdego z nich nazwę regionu i terytorium
         gdzie pracuje. Rezultatem kwerendy LINQ będzie "płaska" lista, więc nazwiska mogą
          się powtarzać (ale każdy rekord będzie unikalny).*/
        var result2 = from e2 in employeesList
            join et2 in employeeTerritoriesList on e2.EmployeeId equals et2.EmployeeId
            join t2 in territoriesList on et2.TerritoryId equals t2.TerritoryId
            join r2 in regionsList on t2.RegionId equals r2.RegionId
            select new
            {
                EmployeLastName = e2.LastName,
                RegionName = r2.RegionDescription,
                TerritoryName = t2.TerritoryDescription
            };
        foreach(var el in result2){
            System.Console.WriteLine($"Pracownik: {el.EmployeLastName}, region: {el.RegionName}, teritory: {el.TerritoryName}");
        }


        /* Zad 3 - wypisz nazwy regionów oraz nazwiska pracowników, którzy pracują w tych
         regionach, pracownicy mają być zagregowani po regionach, rezultatem ma być lista
          regionów z podlistą pracowników (odpowiednik groupjoin).*/
        var result3 = from r3 in regionsList
        join t3 in territoriesList on r3.RegionId equals t3.RegionId
        join et3 in employeeTerritoriesList on t3.TerritoryId equals et3.TerritoryId
        join e3 in employeesList on et3.EmployeeId equals e3.EmployeeId
        group e3.LastName by r3.RegionDescription into regionGroup
        select new
        {
            RegionName = regionGroup.Key,
            Employees = regionGroup.Distinct().ToList() //distinct żeby nie było powtórzeń
        };

        foreach(var el in result3){
            System.Console.Write($"Region: {el.RegionName}, Pracownicy: ");
            foreach(var employee in el.Employees){
                System.Console.Write($"{employee}, ");
            }
            System.Console.WriteLine();
        }

        /* Zad 4 - wypisz nazwy regionów oraz liczbę pracowników w tych regionach.*/

        // Korzystam z wyników poprzedniego zapytania
        foreach(var el in result3){
            System.Console.WriteLine($"Region: {el.RegionName}, Liczba pracowników: {el.Employees.Count()}");
            
        }

        /* Zad 5 - wczytaj do odpowiednich struktur dane z plików orders.csv oraz
         orders_details.csv. Następnie dla każdego pracownika wypisz liczbę dokonanych
          przez niego zamówień, średnią wartość zamówienia oraz maksymalną wartość zamówienia. */
        
        wczytywacz<Order> o = new wczytywacz<Order>();
        List<Order> orderList = o.wczytajListe("./resources/orders.csv", 
        x => new Order(x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7], x[8], x[9], x[10], x[11], x[12], x[13]));
        
        wczytywacz<OrderDetails> od = new wczytywacz<OrderDetails>();
        List<OrderDetails> orderDetailsList = od.wczytajListe("./resources/orders_details.csv",
            x => new OrderDetails(x[0], x[1], x[2], x[3], x[4]));

        var result5 = from e1 in employeesList
        join o1 in orderList on e1.EmployeeId equals o1.EmployeeId
        join od1 in orderDetailsList on o1.OrderId equals od1.OrderId
        group od1 by e1.LastName into ordersDetailsGroup
        select new{
            Employee = ordersDetailsGroup.Key,
            OrdersCount = ordersDetailsGroup.Count(),
            // jak jest null to dodajemy 0
            AvgValueOfOrder = ordersDetailsGroup.Average(order => order.UnitPrice != null ? Double.Parse(order.UnitPrice.Replace('.', ',')) : 0),
            MaxValueOrder = ordersDetailsGroup.Max(order => order.UnitPrice != null ? Double.Parse(order.UnitPrice.Replace('.', ',')) : 0)
        };

        foreach(var employee in result5){
            System.Console.WriteLine($"Pracownik: {employee.Employee}, liczba zamówień: {employee.OrdersCount}, srednia: {employee.AvgValueOfOrder}, Max: {employee.MaxValueOrder}");
        }
    }
}