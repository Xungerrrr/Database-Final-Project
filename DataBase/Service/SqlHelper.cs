using DataBase.Model;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Service
{
    class SqlHelper
    {
        private static SQLiteConnection connection;

        public static void PrepareConnection()
        {
            connection = new SQLiteConnection("car.db"); //创建数据库

            //激活Sqlite外码约束
            using (var statement = connection.Prepare(@"PRAGMA foreign_keys = ON"))
                statement.Step();

            //以下为创建各表的SQL命令
            string FactorySql = @"CREATE TABLE IF NOT EXISTS
               Factory (fid            VARCHAR(10) PRIMARY KEY NOT NULL,
                        fname           VARCHAR(10),
                        faddress        VARCHAR(100)
            );";

            string CarSql = @"CREATE TABLE IF NOT EXISTS
                   Car (cid             VARCHAR(10) PRIMARY KEY NOT NULL,
                        cbrand          VARCHAR(10),
                        cprice          INTEGER,
                        fid             VARCHAR(10) NOT NULL,
                        FOREIGN KEY(fid) REFERENCES Factory(fid) ON UPDATE CASCADE
            );";

            string CustomSql = @"CREATE TABLE IF NOT EXISTS
              Customer (cuid            VARCHAR(10) PRIMARY KEY NOT NULL,
                        cuname          VARCHAR(10),
                        cuaddress       VARCHAR(100)
            );";

            string Factory_trade_dataSql = @"CREATE TABLE IF NOT EXISTS
                Factory_trade_data( 
                        ftid            VARCHAR(10) PRIMARY KEY NOT NULL,
                        ftprice         INTEGER,
                        ftnum           INTEGER,
                        fid             VARCHAR(10) NOT NULL,
                        cid             VARCHAR(10) NOT NULL,
                        FOREIGN KEY(fid) REFERENCES Factory(fid) ON UPDATE CASCADE,
                        FOREIGN KEY(cid) REFERENCES Car(cid) ON UPDATE CASCADE
            );";

            string Customer_trade_dataSql = @"CREATE TABLE IF NOT EXISTS
                Customer_trade_data( 
                        ctid            VARCHAR(10) PRIMARY KEY NOT NULL,
                        ctprice         INTEGER,
                        ctnum           INTEGER,
                        cprofit         INTEGER,
                        cuid            VARCHAR(10) NOT NULL,
                        cid             VARCHAR(10) NOT NULL,
                        FOREIGN KEY(cuid) REFERENCES Customer(cuid) ON UPDATE CASCADE,
                        FOREIGN KEY(cid) REFERENCES Car(cid) ON UPDATE CASCADE
            );";

            string GarageSql = @"CREATE TABLE IF NOT EXISTS
              Garage(   gid             VARCHAR(10) PRIMARY KEY NOT NULL,                     
                        ctnum           INTEGER,
                        cid             VARCHAR(10) NOT NULL, 
                        FOREIGN KEY(cid) REFERENCES Car(cid) ON UPDATE CASCADE 
            );";
            //执行上面的SQL语句
            using (var statement = connection.Prepare(FactorySql))
                statement.Step();
            using (var statement = connection.Prepare(CarSql))
                statement.Step();
            using (var statement = connection.Prepare(CustomSql))
                statement.Step();
            using (var statement = connection.Prepare(Factory_trade_dataSql))
                statement.Step();
            using (var statement = connection.Prepare(Customer_trade_dataSql))
                statement.Step();
            using (var statement = connection.Prepare(GarageSql))
                statement.Step();
        }
		
		//插入工厂数据
		public static String AddFactory(Factory factory, string tableName)
        {
            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(fid, fname, faddress) VALUES (?, ?, ?)"))
                {
                    statement.Bind(1, factory.fid);
                    statement.Bind(2, factory.fname);
                    statement.Bind(3, factory.faddress);
                    statement.Step();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }    
            return connection.ErrorMessage().ToString();
        }
		
		//插入车数据
		public static String AddCar(Car car, string tableName)
        {
            try
            {
                //using (var statement = connection.Prepare("INSERT INTO " + tableName + "(cid, cbrand, cprice, fid) VALUES (?, ?, ?, ?)"))
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(cid, cbrand, cprice, fid) VALUES (?, ?, ?, ?)"))
                {
                    statement.Bind(1, car.cid);
                    statement.Bind(2, car.cbrand);
                    statement.Bind(3, car.cprice);
                    statement.Bind(4, car.fid);
                    statement.Step();
                }
            }
            
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }		
		
		//插入客户的数据
		public static String AddCustomer(Customer customer, string tableName)
        {
            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(cuid, cuname, cuaddress) VALUES (?, ?, ?)"))
                {
                    statement.Bind(1, customer.cuid);
                    statement.Bind(2, customer.cuname);
					statement.Bind(3, customer.cuaddress);
                    statement.Step();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }
		
		//插入工厂交易记录
		public static String AddFactory_trade_data(Factory_trade_data factory_trade_data, string tableName)
        {
            ObservableCollection<Garage> garages = new ObservableCollection<Garage>();
            GetAllGarage(garages, "garage");
            bool w_in = false;
            int temp_ctnum = 0;
            String message = "";
            foreach (Garage garage in garages)
            {
                if(garage.cid == factory_trade_data.cid)
                {
                    w_in = true;
                    temp_ctnum = garage.ctnum;
                    Debug.Write(temp_ctnum);
                    break;
                }
            }
            if (w_in)
            {
                String UPDATE = "UPDATE " + "garage" + " SET ctnum = ? WHERE cid = ?";
                int ctnum = temp_ctnum + factory_trade_data.ftnum;
                try
                {
                    using (var statement = connection.Prepare(UPDATE))
                    {
                        statement.Bind(1, ctnum);
                        statement.Bind(2, factory_trade_data.cid);
                        statement.Step();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                message = connection.ErrorMessage().ToString();
            }
            else
            {
                Garage garage = new Garage { gid = factory_trade_data.cid, cid = factory_trade_data.cid, ctnum = factory_trade_data.ftnum };
                message =  AddGarage(garage, "Garage");
            }

            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(ftid, ftprice, ftnum, fid, cid) VALUES (?, ?, ?, ?, ?)"))
                {
                    statement.Bind(1, factory_trade_data.ftid);
                    statement.Bind(2, factory_trade_data.ftprice);
					statement.Bind(3, factory_trade_data.ftnum);
                    statement.Bind(4, factory_trade_data.fid);
                    statement.Bind(5, factory_trade_data.cid);
                    statement.Step();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            if (message == "not an error" && connection.ErrorMessage().ToString() == "not an error")
                return message;
            else if (connection.ErrorMessage().ToString() != "not an error" && message != "not an error")
                return message + "\n" + connection.ErrorMessage().ToString();
            else if (message == "not an error")
                return connection.ErrorMessage().ToString();
            else
                return message;
        }



        //插入用户交易记录
        public static String AddCustomer_trade_data(Customer_trade_data customer_trade_data, string tableName)
        {
            ObservableCollection<Garage> garages = new ObservableCollection<Garage>();
            GetAllGarage(garages, "garage");
            Car temp_car = new Car();
            int temp_ctnum = 0;
            foreach (Garage temp in garages)
            {
                if (temp.cid == customer_trade_data.cid)
                {
                    temp_ctnum = temp.ctnum;
                }
            }
            String UPDATE = "UPDATE " + "garage" + " SET ctnum = ? WHERE cid = ?";
            try
            {
                using (var statement = connection.Prepare(UPDATE))
                {
                    statement.Bind(1, (temp_ctnum - customer_trade_data.ctnum));
                    statement.Bind(2, customer_trade_data.cid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            String message = connection.ErrorMessage().ToString();

            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(ctid, ctprice, ctnum, cuid, cid, cprofit) VALUES (?, ?, ?, ?, ?, ?)"))
                {
                    statement.Bind(1, customer_trade_data.ctid);
                    statement.Bind(2, customer_trade_data.ctprice);
                    statement.Bind(3, customer_trade_data.ctnum);
                    statement.Bind(4, customer_trade_data.cuid);
                    statement.Bind(5, customer_trade_data.cid);
                    statement.Bind(6, customer_trade_data.cprofit);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            if (message == "not an error" && connection.ErrorMessage().ToString() == "not an error")
                return message;
            else if (connection.ErrorMessage().ToString() != "not an error" && message != "not an error")
                return message + "\n" + connection.ErrorMessage().ToString();
            else if (message == "not an error")
                return connection.ErrorMessage().ToString();
            else
                return message;
        }


        //插入用户交易记录
        public static String AddGarage(Garage garage, string tableName)
        {
            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(gid, ctnum, cid) VALUES (?, ?, ?)"))
                {
                    statement.Bind(1, garage.gid);
                    statement.Bind(2, garage.ctnum);
                    statement.Bind(3, garage.cid);
                    statement.Step();
                }
                Debug.WriteLine(connection.ErrorMessage().ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }


        //获取信息
        public static String GetAllFactory(ObservableCollection<Factory> factorys, string tableName, string searchStr = "")
        {
            factorys.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE fid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Factory factory = new Factory();
                        factory.fid = (string)statement[0];
                        factory.fname = (string)statement[1];
                        factory.faddress = (string)statement[2];
                        factorys.Add(factory);
                    }
                }
                return connection.ErrorMessage().ToString();
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Factory factory = new Factory();
                    factory.fid = (string)statement[0];
                    factory.fname = (string)statement[1];
                    factory.faddress = (string)statement[2];
                    factorys.Add(factory);
                }
            }
            return connection.ErrorMessage().ToString();
        }


        //获取车信息
        public static String GetAllCar(ObservableCollection<Car> cars, string tableName, string searchStr = "")
        {
            cars.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE cid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Car car = new Car();
                        car.cid = (string)statement[0];
                        car.cbrand = (string)statement[1];
                        car.cprice = (int)(long)statement[2];
                        car.fid = (string)statement[3];
                        cars.Add(car);
                    }
                }
                return connection.ErrorMessage().ToString();
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Car car = new Car();
                    car.cid = (string)statement[0];
                    car.cbrand = (string)statement[1];
                    car.cprice = (int)(long)statement[2];
                    car.fid = (string)statement[3];
                    cars.Add(car);
                }
            }
            return connection.ErrorMessage().ToString();
        }

        //获取用户信息
        public static String GetAllCustomer(ObservableCollection<Customer> customers, string tableName, string searchStr = "")
        {
            customers.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE cuid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Customer customer = new Customer();
                        customer.cuid = (string)statement[0];
                        customer.cuname = (string)statement[1];
                        customer.cuaddress = (string)statement[2];
                        customers.Add(customer);
                    }
                }
                return connection.ErrorMessage().ToString();
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Customer customer = new Customer();
                    customer.cuid = (string)statement[0];
                    customer.cuname = (string)statement[1];
                    customer.cuaddress = (string)statement[2];
                    customers.Add(customer);
                }
            }
            return connection.ErrorMessage().ToString();
        }


        //获取用户交易信息
        public static String GetAllCustomer_trade_data(ObservableCollection<Customer_trade_data> customer_Trade_Datas, string tableName, string searchStr = "")
        {
            customer_Trade_Datas.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE ctid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Customer_trade_data customer_Trade_Data = new Customer_trade_data();
                        customer_Trade_Data.ctid = (string)statement[0];
                        customer_Trade_Data.ctprice = (int)(long)statement[1];
                        customer_Trade_Data.ctnum = (int)(long)statement[2];
                        customer_Trade_Data.cuid = (string)statement[3];
                        customer_Trade_Data.cid = (string)statement[4];
                        customer_Trade_Data.cprofit = (int)(long)statement[5];
                        customer_Trade_Datas.Add(customer_Trade_Data);
                    }
                }
                return connection.ErrorMessage().ToString();
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Customer_trade_data customer_Trade_Data = new Customer_trade_data();
                    customer_Trade_Data.ctid = (string)statement[0];
                    customer_Trade_Data.ctprice = (int)(long)statement[1];
                    customer_Trade_Data.ctnum = (int)(long)statement[2];
                    customer_Trade_Data.cuid = (string)statement[3];
                    customer_Trade_Data.cid = (string)statement[4];
                    customer_Trade_Data.cprofit = (int)(long)statement[5];
                    customer_Trade_Datas.Add(customer_Trade_Data);
                }
            }
            return connection.ErrorMessage().ToString();
        }



        //获取厂商交易信息
        public static String GetAllFactory_trade_data(ObservableCollection<Factory_trade_data> factory_Trade_Datas, string tableName, string searchStr = "")
        {
            factory_Trade_Datas.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE ftid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Factory_trade_data factory_Trade_Data = new Factory_trade_data();
                        factory_Trade_Data.ftid = (string)statement[0];
                        factory_Trade_Data.ftprice = (int)(long)statement[1];
                        factory_Trade_Data.ftnum = (int)(long)statement[2];
                        factory_Trade_Data.fid = (string)statement[3];
                        factory_Trade_Data.cid = (string)statement[4];
                        factory_Trade_Datas.Add(factory_Trade_Data);
                    }
                }
                return connection.ErrorMessage().ToString();
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Factory_trade_data factory_Trade_Data = new Factory_trade_data();
                    factory_Trade_Data.ftid = (string)statement[0];
                    factory_Trade_Data.ftprice = (int)(long)statement[1];
                    factory_Trade_Data.ftnum = (int)(long)statement[2];
                    factory_Trade_Data.fid = (string)statement[3];
                    factory_Trade_Data.cid = (string)statement[4];
                    factory_Trade_Datas.Add(factory_Trade_Data);
                }
            }
            return connection.ErrorMessage().ToString();
        }


        //获取车库信息
        public static String GetAllGarage(ObservableCollection<Garage> garages, string tableName, string searchStr = "")
        {
            garages.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql +  " WHERE gid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Garage garage = new Garage();
                        garage.gid = (string)statement[0];
                        garage.ctnum = (int)(long)statement[1];
                        garage.cid = (string)statement[2];
                        garages.Add(garage);
                    }
                }
                return connection.ErrorMessage().ToString();
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Garage garage = new Garage();
                    garage.gid = (string)statement[0];
                    garage.ctnum = (int)(long)statement[1];
                    garage.cid = (string)statement[2];
                    garages.Add(garage);
                }
            }
            return connection.ErrorMessage().ToString();
        }


        //删除信息


        //删除Factory
        public static String DeleteFactory(string tableName, Factory factory)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "fid = ?" ))
                {
                    statement.Bind(1, factory.fid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        //删除customer
        public static String DeleteCustomer(string tableName, Customer customer)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "cuid = ?"))
                {
                    statement.Bind(1, customer.cuid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String DeleteCar(string tableName, Car car)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "cid = ?"))
                {
                    statement.Bind(1, car.cid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String DeleteCustomer_trade_data(string tableName, Customer_trade_data customer_Trade_Data)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "ctid = ?"))
                {
                    statement.Bind(1, customer_Trade_Data.ctid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String DeleteFactory_trade_data(string tableName, Factory_trade_data factory_Trade_Data)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "ftid = ?"))
                {
                    statement.Bind(1, factory_Trade_Data.ftid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String DeleteGarage(string tableName, Garage garage)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "gid = ?"))
                {
                    statement.Bind(1, garage.gid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String UpdateGarage(string tableName, Garage garage)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET ctnum = ?, cid = ? WHERE gid = ?"))
                {
                    statement.Bind(1, garage.ctnum);
                    statement.Bind(2, garage.cid);
                    statement.Bind(3, garage.gid);
                    statement.Step();
                }
                Debug.WriteLine(connection.ErrorMessage().ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String UpdateCar(string tableName, Car car)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET cbrand = ?, cprice = ? WHERE cid = ?"))
                {
                    statement.Bind(1, car.cbrand);
                    statement.Bind(2, car.cprice);
                    statement.Bind(3, car.cid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();
        }

        public static String UpdateFactory(string tableName, Factory factory)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET fname = ?, faddress = ? WHERE fid = ?"))
                {
                    statement.Bind(1, factory.fname);
                    statement.Bind(2, factory.faddress);
                    statement.Bind(3, factory.fid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();

        }

        public static String UpdateCustomer(string tableName, Customer customer)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET cuaddress = ?, cuname = ? WHERE cuid = ?"))
                {
                    statement.Bind(1, customer.cuaddress);
                    statement.Bind(2, customer.cuname);
                    statement.Bind(3, customer.cuid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();

        }
        public static String UpdateFactory_trade_data(string tableName, Factory_trade_data factory_Trade_Data)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET ftprice = ?, ftnum = ? WHERE ftid = ?"))
                {
                    statement.Bind(1, factory_Trade_Data.ftprice);
                    statement.Bind(2, factory_Trade_Data.ftnum);
                    statement.Bind(3, factory_Trade_Data.ftid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();

        }
        public static String UpdateCustomer_trade_data(string tableName, Customer_trade_data customer_Trade_Data)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET ctprice = ?, ctnum = ? WHERE ctid = ?"))
                {
                    statement.Bind(1, customer_Trade_Data.ctprice);
                    statement.Bind(2, customer_Trade_Data.ctnum);
                    statement.Bind(3, customer_Trade_Data.ctid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return connection.ErrorMessage().ToString();

        }
        public static int GetTotal_profit(string tableName) {
            string sql = "SELECT SUM(cprofit) FROM " + tableName;
            using (var statement = connection.Prepare(sql)) {
                statement.Step();
                
                return statement[0] == null ? 0 : (int)(long)statement[0];
            }
        }
    }
}
