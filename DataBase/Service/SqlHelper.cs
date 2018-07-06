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
            connection = new SQLiteConnection("car.db");
           
            string FactorSql = @"CREATE TABLE IF NOT EXISTS
                Factor( fid             VARCHAR(10) PRIMARY KEY NOT NULL,
                        fname           VARCHAR(10),
                        faddress        VARCHAR(5)
            );";

            string CarSql = @"CREATE TABLE IF NOT EXISTS
                Car(    cid             VARCHAR(10) PRIMARY KEY NOT NULL,
                        cbrand          VARCHAR(10),
                        cprice          INTEGER,
                        fid             VARCHAR(10) REFERENCES Factor(fid) ON UPDATE CASCADE
            );";

            string CustomSql = @"CREATE TABLE IF NOT EXISTS
                Customer( cuid          VARCHAR(10) PRIMARY KEY NOT NULL,
                        cuname          VARCHAR(10),
                        cuaddress       VARCHAR(5)
            );";

            string Factor_trade_dataSql = @"CREATE TABLE IF NOT EXISTS
                Factor_trade_data( 
                        ftid            VARCHAR(10) PRIMARY KEY NOT NULL,
                        ftprice         INTEGER,
                        ftnum           INTEGER,
                        fid             VARCHAR(10) REFERENCES Factor(fid) ON UPDATE CASCADE,
                        cid             VARCHAR(10) REFERENCES Car(cid) ON UPDATE CASCADE               
            );";

            string Customer_trade_dataSql = @"CREATE TABLE IF NOT EXISTS
                Customer_trade_data( 
                        ctid            VARCHAR(10) PRIMARY KEY NOT NULL,
                        ctprice         INTEGER,
                        ctnum           INTEGER,
                        cuid            VARCHAR(10) REFERENCES Customer(cuid) ON UPDATE CASCADE,
                        cid             VARCHAR(10) REFERENCES Car(cid) ON UPDATE CASCADE,
                        cprofit         INTEGER
            );";

            string GarbageSql = @"CREATE TABLE IF NOT EXISTS
                Garbage( 
                        gid             VARCHAR(10) PRIMARY KEY NOT NULL,                     
                        ctnum           INTEGER,
                        cid             VARCHAR(10) REFERENCES Car(cid) ON UPDATE CASCADE               
            );";

            using (var statement = connection.Prepare(FactorSql))
                statement.Step();
            using (var statement = connection.Prepare(CarSql))
                statement.Step();
            using (var statement = connection.Prepare(CustomSql))
                statement.Step();
            using (var statement = connection.Prepare(Factor_trade_dataSql))
                statement.Step();
            using (var statement = connection.Prepare(Customer_trade_dataSql))
                statement.Step();
            using (var statement = connection.Prepare(GarbageSql))
                statement.Step();
        }
		
		//插入工厂数据
		public static void AddFactor(Factor factor, string tableName)
        {
            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(fid, fname, faddress) VALUES (?, ?, ?)"))
                {
                    statement.Bind(1, factor.fid);
                    statement.Bind(2, factor.fname);
                    statement.Bind(3, factor.faddress);
                    statement.Step();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
		
		//插入车数据
		public static void AddCar(Car car, string tableName)
        {
            try
            {
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
        }		
		
		//插入客户的数据
		public static void AddCustomer(Customer customer, string tableName)
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
        }
		
		//插入工厂交易记录
		public static void AddFactor_trade_data(Factor_trade_data factor_trade_data, string tableName)
        {
            ObservableCollection<Garbage> garbages = new ObservableCollection<Garbage>();
            GetAllGarbage(garbages, "garbage");
            Car temp_car = new Car();
            int temp_ctnum = 0;
            foreach(Garbage temp in garbages)
            {
                if(temp.cid == factor_trade_data.cid)
                {
                    temp_ctnum = temp.ctnum;
                }
            }
            String UPDATE = "UPDATE " + "garbage" + " SET ctnum = ? WHERE cid = ?";
            try
            {
                using (var statement = connection.Prepare(UPDATE))
                {
                    statement.Bind(1, (temp_ctnum + factor_trade_data.ftnum));
                    statement.Bind(2, factor_trade_data.cid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }


            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(ftid, ftprice, ftnum, fid, cid) VALUES (?, ?, ?, ?, ?)"))
                {
                    statement.Bind(1, factor_trade_data.ftid);
                    statement.Bind(2, factor_trade_data.ftprice);
					statement.Bind(3, factor_trade_data.ftnum);
                    statement.Bind(4, factor_trade_data.fid);
                    statement.Bind(5, factor_trade_data.cid);
                    statement.Step();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }



        //插入用户交易记录
        public static void AddCustomer_trade_data(Customer_trade_data customer_trade_data, string tableName)
        {
            ObservableCollection<Garbage> garbages = new ObservableCollection<Garbage>();
            GetAllGarbage(garbages, "garbage");
            Car temp_car = new Car();
            int temp_ctnum = 0;
            foreach (Garbage temp in garbages)
            {
                if (temp.cid == customer_trade_data.cid)
                {
                    temp_ctnum = temp.ctnum;
                }
            }
            String UPDATE = "UPDATE " + "garbage" + " SET ctnum = ? WHERE cid = ?";
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
        }


        //插入用户交易记录
        public static void AddGarbage(Garbage garbage, string tableName)
        {
            try
            {
                using (var statement = connection.Prepare("INSERT INTO " + tableName + "(gid, ctnum, cid) VALUES (?, ?, ?)"))
                {
                    statement.Bind(1, garbage.gid);
                    statement.Bind(2, garbage.ctnum);
                    statement.Bind(3, garbage.cid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        //获取信息
        public static void GetAllFactor(ObservableCollection<Factor> factors, string tableName, string searchStr = "")
        {
            factors.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE fid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Factor factor = new Factor();
                        factor.fid = (string)statement[0];
                        factor.fname = (string)statement[1];
                        factor.faddress = (string)statement[2];
                        factors.Add(factor);
                    }
                }
                return;
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Factor factor = new Factor();
                    factor.fid = (string)statement[0];
                    factor.fname = (string)statement[1];
                    factor.faddress = (string)statement[2];
                    factors.Add(factor);
                }
            }
        }


        //获取车信息
        public static void GetAllCar(ObservableCollection<Car> cars, string tableName, string searchStr = "")
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
                return;
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
        }

        //获取用户信息
        public static void GetAllCustomer(ObservableCollection<Customer> customers, string tableName, string searchStr = "")
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
                return;
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
        }


        //获取用户交易信息
        public static void GetAllCustomer_trade_data(ObservableCollection<Customer_trade_data> customer_Trade_Datas, string tableName, string searchStr = "")
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
                return;
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
        }



        //获取厂商交易信息
        public static void GetAllFactor_trade_data(ObservableCollection<Factor_trade_data> factor_Trade_Datas, string tableName, string searchStr = "")
        {
            factor_Trade_Datas.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql + " WHERE ftid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Factor_trade_data factor_Trade_Data = new Factor_trade_data();
                        factor_Trade_Data.ftid = (string)statement[0];
                        factor_Trade_Data.ftprice = (int)(long)statement[1];
                        factor_Trade_Data.ftnum = (int)(long)statement[2];
                        factor_Trade_Data.fid = (string)statement[3];
                        factor_Trade_Data.cid = (string)statement[4];
                        factor_Trade_Datas.Add(factor_Trade_Data);
                    }
                }
                return;
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Factor_trade_data factor_Trade_Data = new Factor_trade_data();
                    factor_Trade_Data.ftid = (string)statement[0];
                    factor_Trade_Data.ftprice = (int)(long)statement[1];
                    factor_Trade_Data.ftnum = (int)(long)statement[2];
                    factor_Trade_Data.fid = (string)statement[3];
                    factor_Trade_Data.cid = (string)statement[4];
                    factor_Trade_Datas.Add(factor_Trade_Data);
                }
            }
        }


        //获取车库信息
        public static void GetAllGarbage(ObservableCollection<Garbage> garbages, string tableName, string searchStr = "")
        {
            garbages.Clear();
            string sql = "SELECT * FROM " + tableName;
            if (searchStr != "")
            {
                sql = sql +  " WHERE gid = " + searchStr;
                using (var statement = connection.Prepare(sql))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        Garbage garbage = new Garbage();
                        garbage.gid = (string)statement[0];
                        garbage.ctnum = (int)(long)statement[1];
                        garbage.cid = (string)statement[2];
                        garbages.Add(garbage);
                    }
                }
                return;
            }
            using (var statement = connection.Prepare(sql))
            {
                while (statement.Step() == SQLiteResult.ROW)
                {
                    Garbage garbage = new Garbage();
                    garbage.gid = (string)statement[0];
                    garbage.ctnum = (int)(long)statement[1];
                    garbage.cid = (string)statement[2];
                    garbages.Add(garbage);
                }
            }
        }


        //删除信息


        //删除Factor
        public static void DeleteFactor(string tableName, Factor factor)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "fid = ?" ))
                {
                    statement.Bind(1, factor.fid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        //删除customer
        public static void DeleteCustomer(string tableName, Customer customer)
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
        }

        public static void DeleteCar(string tableName, Car car)
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
        }

        public static void DeleteCustomer_trade_data(string tableName, Customer_trade_data customer_Trade_Data)
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
        }

        public static void DeleteFactor_trade_data(string tableName, Factor_trade_data factor_Trade_Data)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "ftid = ?"))
                {
                    statement.Bind(1, factor_Trade_Data.ftid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void DeleteGarbage(string tableName, Garbage garbage)
        {
            try
            {
                using (var statement = connection.Prepare("DELETE FROM " + tableName + " WHERE " + "gid = ?"))
                {
                    statement.Bind(1, garbage.gid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateGarbage(string tableName, Garbage garbage)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET ctnum = ?, cid = ? WHERE gid = ?"))
                {
                    statement.Bind(1, garbage.ctnum);
                    statement.Bind(2, garbage.cid);
                    statement.Bind(3, garbage.gid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void UpdateCar(string tableName, Car car)
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
        }

        public static void UpdateFactor(string tableName, Factor factor)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET fname = ?, faddress = ? WHERE fid = ?"))
                {
                    statement.Bind(1, factor.fname);
                    statement.Bind(2, factor.faddress);
                    statement.Bind(3, factor.fid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        public static void UpdateCustomer(string tableName, Customer customer)
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

        }
        public static void UpdateFactor_trade_data(string tableName, Factor_trade_data factor_Trade_Data)
        {
            try
            {
                using (var statement = connection.Prepare("UPDATE " + tableName + " SET ftprice = ?, ftnum = ? WHERE ftid = ?"))
                {
                    statement.Bind(1, factor_Trade_Data.ftprice);
                    statement.Bind(2, factor_Trade_Data.ftnum);
                    statement.Bind(3, factor_Trade_Data.ftid);
                    statement.Step();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
        public static void UpdateCustomer_trade_data(string tableName, Customer_trade_data customer_Trade_Data)
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

        }
        public static int GetTotal_profit(string tableName) {
            string sql = "SELECT SUM(cprofit) FROM " + tableName;
            using (var statement = connection.Prepare(sql)) {
                statement.Step();
                return (int)(long)statement[0];
            }
        }
    }
}
