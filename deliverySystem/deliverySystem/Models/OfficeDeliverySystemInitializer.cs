namespace deliverySystem.Models
{
    using System.Collections.Generic;
    using System;

    public class OfficeDeliverySystemInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<OfficeDeliveryContext>
    {
        protected override void Seed(OfficeDeliveryContext context)
        {
            int departmentId = 1;
            var departments = new List<Department>
            {
                new Department{Id=departmentId++, Name="Department 1" },
                new Department{Id=departmentId++, Name="Department 2" },
                new Department{Id=departmentId++, Name="Department 3" }
            };
            
            int userId = 1;
            var users = new List<User>
            {
                new User{Id=userId,FirstName="Admin", LastName=userId.ToString(), Birthdate=DateTime.Now, Block = 3, Department=departments[0],Email="admin@gmail.com", Floor =2, Office="Office", Password="123",Role=Role.Admin},
                new User{Id=userId,FirstName="Client", LastName=userId.ToString(), Birthdate=DateTime.Now, Block = 3, Department=departments[2],Email="customer@gmail.com", Floor =2, Office="Office", Password="123",Role=Role.Client},
                new User{Id=userId,FirstName="Cafeteria", LastName=userId.ToString(), Birthdate=DateTime.Now, Block = 3, Department=departments[0],Email="cafeteria@gmail.com", Floor =2, Office="Office", Password="123",Role=Role.DeliveryMan,Category=Category.Cafeteria, DeliveryFree = true},
                new User{Id=userId,FirstName="Printer", LastName=userId.ToString(), Birthdate=DateTime.Now, Block = 3, Department=departments[1],Email="printer@gmail.com", Floor =2, Office="Office", Password="123",Role=Role.DeliveryMan,Category=Category.Printer, DeliveryFree = true},              
            };

            int itemId = 1;
            var items = new List<Item> {
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category=Category.Cafeteria, File="item1.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category=Category.Cafeteria, File="item2.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category = Category.Cafeteria, File="item3.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category = Category.Cafeteria, File="item1.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category=Category.Printer, File="item2.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category = Category.Cafeteria, File="item3.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category=Category.Printer, File="item1.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category = Category.Cafeteria, File="item2.jpg"},
                new Item{Id=itemId++, Name="item"+itemId,Price=itemId*10, Category=Category.Printer, File="item3.jpg"},
            };

            users.ForEach(s => context.Users.Add(s));
            departments.ForEach(s => context.Departments.Add(s));
            items.ForEach(s => context.Items.Add(s));

            context.SaveChanges();
        }
    }
}