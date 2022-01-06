using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using DalApi;
using DO;
using System.Security.Cryptography;

namespace Dal
{
    class DLXML
    {
        #region singelton

        static readonly DLXML instance = new DLXML();
        static DLXML() { }
        DLXML() { }
        public static DLXML Instance => instance;
        #endregion
        string parcelPath = @"ParcelsXml.xml";//XMLSerializer
        string stationPath = @"StationsXml.xml";  //XElement
        string customerPath = @"CustomersXml.xml";//XElement
        string dronePath = @"DronesXml.xml";//XMLSerializer
        string usersPath = @"UsersXml.xml";//XMLSerializer
        string droneChargePath = @"DroneChargeXml.xml";
         



        #region station
        public void AddStations(Station station)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElem = (from stations in stationsRoot.Elements()
                                  where (stations.Element("ID").Value == station.ToString()) 
                                  select stations).FirstOrDefault();
            if (stationElem != null)
                throw new InVaildIdException("The two following stations already exist in the system");
            XElement newStation = new XElement("Station"
                , new XElement("ID", station.ID),
                new XElement("Name", station.Name),
                new XElement("Longitude", station.Longitude.ToString()),
                new XElement("Lattitude", station.Lattitude.ToString()),
                new XElement("ChargeSlots", station.ChargeSlots.ToString()));
                stationsRoot.Add(newStation);
            XMLTools.SaveListToXMLElement(stationsRoot, stationPath);
        }


        public void DeleteStation(int stationID )
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElem = (from stations in stationsRoot.Elements()
                                  where (stations.Element("ID").Value == stationID.ToString())
                                  
                                  select stations).FirstOrDefault();
            if (stationElem == null)
                throw new InVaildIdException("the station dosn't exists");

            stationElem.Remove();
            XMLTools.SaveListToXMLElement(stationsRoot, stationPath);
        }

         

        public IEnumerable<Station> GetStations(Func<Station, bool> predicate = null)
        {
            List<Station> listOfAllStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            if (predicate == null)
                return listOfAllStations;
            return listOfAllStations.Where(predicate);


        }
 

        public Station GetStation(int stationID )
        {
            var listOfStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station station = listOfStation.Find(x => x.ID == stationID);
            if (!listOfStation.Exists(x => x.ID == station.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return station;

        }

        public void UpdateStations(Station station)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stations = (from stationElem in stationsRoot.Elements()
                               where (stationElem.Element("ID").Value == station.ID.ToString())
                              
                               select stationElem).FirstOrDefault();
            if (stations == null)
                throw new InVaildIdException("the station dosn't exists");
     
            stations.Element("Name").Value = station.Name.ToString();
            stations.Element("ChargeSlots").Value = station.ChargeSlots.ToString();
        }

        #endregion

         
         

        #region users
        public void AddUser(User userToAdd)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            if (listOfAllUsers.Find(x => x.UserName == userToAdd.UserName) != null)
                throw new AlreadyExistExeption("This user already exist");
            listOfAllUsers.Add(userToAdd);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }

        public IEnumerable<User> GetPartOfUsers(Predicate<User> UserCondition)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            var list = from user in listOfAllUsers
                       where (UserCondition(user))
                       select user;
            return list;
        }

        public void DeleteUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User user = listOfAllUsers.Find(x => x.UserName == userName);
            if (user == null)
                throw new InVaildIdException("the user dosn't exist in system");
            listOfAllUsers.Remove(user);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }

        public IEnumerable<User> GetAllUsers()
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            return listOfAllUsers;
        }
        public User GetAUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == userName);
            if (myUser != null)
                return myUser;
            throw new InVaildIdException("the user doesn't exists in system");
        }

        public void UpdateUser(User usertoUpdate)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == usertoUpdate.UserName);
            if (myUser == null)
                throw new InVaildIdException("This user doesn't exist in the system");
            myUser.Password = usertoUpdate.Password;
            myUser.Worker = usertoUpdate.Worker;
            myUser.IsActive = usertoUpdate.IsActive;
            myUser.UserName = usertoUpdate.UserName;
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }

        #endregion

        #region customer
        public void AddCustomer(Customer customer)
        {
            var listOfCustomer= XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new AlreadyExistExeption("The bus already exist in the system");
            listOfCustomer.Add(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        public void DeleteCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new InVaildIdException("The customer doesn't exist in system");
            listOfCustomer.Remove(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);

        }

        public Customer GetCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))
                
            throw new InVaildIdException("The customer doesn't exist in system");
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            return XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
        }

        public IEnumerable<Customer> GetCustomers(Func<Customer,bool> func=null)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            var list = from customer in listOfCustomer
                       where (func(customer))
                       select customer;
            return list;
        }

        public void UpdateCustomer(Customer customer)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer myCustomer = listOfCustomer.Find(x => x.ID == customer.ID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))

                throw new InVaildIdException("This customer doesn't exist in the system");
            myCustomer.ID = customer.ID;
            myCustomer.Name = customer.Name;
            myCustomer.Phone = customer.Phone;
            myCustomer.Lattitude = customer.Lattitude;
            myCustomer.Longitude = customer.Longitude;
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        #endregion

        #region lines 
        public void AddBusLine(BusLine busLineToAdd)
        {
            XElement busLineRoot = XMLTools.LoadListFromXMLElement(linesPath);
            XElement configurationRoot = XMLTools.LoadListFromXMLElement(configurationPath);

            XElement newLineElem = new XElement("BusLine"
              , new XElement("BusLineIndentificator", configurationRoot.Element("BusLineID").Value),
              new XElement("LineNumber", busLineToAdd.LineNumber),
              new XElement("LineArea", busLineToAdd.LineArea.ToString()),
              new XElement("FirstLineStation", busLineToAdd.FirstLineStation),
              new XElement("LastLineStation", busLineToAdd.LastLineStation)
              );

            busLineRoot.Add(newLineElem);
            XMLTools.SaveListToXMLElement(busLineRoot, linesPath);

            configurationRoot.Element("BusLineID").Value = (Convert.ToInt32(configurationRoot.Element("BusLineID").Value) + 1).ToString();
            XMLTools.SaveListToXMLElement(configurationRoot, configurationPath);
        }


        public void DeleteBusLine(int busIndentificatorToDelete)
        {
            XElement busLineRoot = XMLTools.LoadListFromXMLElement(linesPath);
            var lineToDelete = (from line in busLineRoot.Elements()
                                where (line.Element("BusLineIndentificator").Value == busIndentificatorToDelete.ToString())
                                select line).FirstOrDefault();

            if (lineToDelete == null)
                throw new DoesntExistException("The line to delete doesn't exist in system");
            lineToDelete.Remove();
            XMLTools.SaveListToXMLElement(busLineRoot, linesPath);
        }


        public BusLine GetABusLine(int busIndetificator)
        {
            XElement busLineRoot = XMLTools.LoadListFromXMLElement(linesPath);
            var lineToDelete = (from line in busLineRoot.Elements()
                                where (line.Element("BusLineIndentificator").Value == busIndetificator.ToString())
                                select line).FirstOrDefault();
            if (lineToDelete != null)
                return new BusLine
                {
                    BusLineIndentificator = Convert.ToInt32(lineToDelete.Element("BusLineIndentificator").Value),
                    FirstLineStation = Convert.ToInt32(lineToDelete.Element("FirstLineStation").Value),
                    LastLineStation = Convert.ToInt32(lineToDelete.Element("LastLineStation").Value),
                    LineNumber = Convert.ToInt32(lineToDelete.Element("LineNumber").Value),
                    LineArea = lineToDelete.Element("LineArea").Value.ParseToArea()
                };
            throw new DoesntExistException("The line doesn't exist in system");
        }

        public IEnumerable<BusLine> GetAllBusLines()
        {
            XElement busLineRoot = XMLTools.LoadListFromXMLElement(linesPath);
            var listOfAllLines = from line in busLineRoot.Elements()
                                 select new BusLine
                                 {
                                     BusLineIndentificator = Convert.ToInt32(line.Element("BusLineIndentificator").Value),
                                     FirstLineStation = Convert.ToInt32(line.Element("FirstLineStation").Value),
                                     LastLineStation = Convert.ToInt32(line.Element("LastLineStation").Value),
                                     LineNumber = Convert.ToInt32(line.Element("LineNumber").Value),
                                     LineArea = line.Element("LineArea").Value.ParseToArea()
                                 };
            return listOfAllLines;

        }

        public IEnumerable<BusLine> GetPartOfBuseLines(Predicate<BusLine> BusLineCondition)
        {
            XElement busLineRoot = XMLTools.LoadListFromXMLElement(linesPath);
            var listOfLines = from line in busLineRoot.Elements()
                              let busLineDO = new BusLine
                              {
                                  BusLineIndentificator = Convert.ToInt32(line.Element("BusLineIndentificator").Value),
                                  FirstLineStation = Convert.ToInt32(line.Element("FirstLineStation").Value),
                                  LastLineStation = Convert.ToInt32(line.Element("LastLineStation").Value),
                                  LineNumber = Convert.ToInt32(line.Element("LineNumber").Value),
                                  LineArea = line.Element("LineArea").Value.ParseToArea()
                              }
                              where BusLineCondition(busLineDO)
                              select busLineDO;
            return listOfLines;
        }

        public void UpdateBusLine(BusLine busLineToUpdate)
        {
            XElement busLineRoot = XMLTools.LoadListFromXMLElement(linesPath);
            var myLine = (from line in busLineRoot.Elements()
                          where (line.Element("BusLineIndentificator").Value == busLineToUpdate.BusLineIndentificator.ToString())
                          select line).FirstOrDefault();

            if (myLine == null)
                throw new DoesntExistException("This bus line doesn't exist in the system");

            myLine.Element("FirstLineStation").Value = busLineToUpdate.FirstLineStation.ToString();
            myLine.Element("LastLineStation").Value = busLineToUpdate.LastLineStation.ToString();
            myLine.Element("LineArea").Value = busLineToUpdate.LineArea.ToString();
            myLine.Element("LineNumber").Value = busLineToUpdate.LineNumber.ToString();

            XMLTools.SaveListToXMLElement(busLineRoot, linesPath);
        }

        #endregion

        #region tripLines
        public void AddLineTrip(LineTrip lineTripToAdd)
        {
            XElement linesTripRoot = XMLTools.LoadListFromXMLElement(linesTripPath);

            if (lineTripToAdd.TimeFirstLineExit >= new TimeSpan(24, 0, 0) || lineTripToAdd.TimeLastLineExit >= new TimeSpan(24, 0, 0))
                throw new InvalidInputException("the hour is invalid");

            var lineTripList = GetAllLinesTimes();// all the list of the lines trip..

            LineTrip lineTimes = lineTripToAdd;
            //check if there is a bus exit that collision with the new times:
            foreach (var r in lineTripList)
            {
                if (lineTimes.BusLineIndentificator == r.BusLineIndentificator)
                {
                    //must be collision.. both drive in 00:00:00..
                    if (lineTimes.TimeFirstLineExit > lineTimes.TimeLastLineExit && r.TimeFirstLineExit > r.TimeLastLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                    //both drive in time 00:00:00- 23:59:59
                    if (lineTimes.TimeFirstLineExit < lineTimes.TimeLastLineExit && r.TimeFirstLineExit < r.TimeLastLineExit)
                    {
                        if (lineTimes.TimeFirstLineExit >= r.TimeFirstLineExit && lineTimes.TimeFirstLineExit < r.TimeLastLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");
                        if (lineTimes.TimeLastLineExit > r.TimeFirstLineExit && lineTimes.TimeLastLineExit < r.TimeLastLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");
                        if (lineTimes.TimeFirstLineExit < r.TimeFirstLineExit && lineTimes.TimeLastLineExit > r.TimeLastLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");
                    }
                    //r drive from "day" to other "day" and lineTimes in one day..
                    else if (r.TimeFirstLineExit > r.TimeLastLineExit && lineTimes.TimeFirstLineExit < lineTimes.TimeLastLineExit)
                    {
                        if (r.TimeLastLineExit > lineTimes.TimeFirstLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");
                        if (r.TimeFirstLineExit < lineTimes.TimeLastLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");

                        //if (r.TimeFirstLineExit < lineTriptoUpdate.TimeFirstLineExit || r.TimeLastLineExit > lineTriptoUpdate.TimeLastLineExit)
                        //    throw new AlreadyExistException("The times of the line that has chose already have defination");
                    }
                    // lineTimes drive from "day" to other "day" and r in one day..
                    else if (r.TimeFirstLineExit < r.TimeLastLineExit && lineTimes.TimeFirstLineExit > lineTimes.TimeLastLineExit)
                    {
                        if (r.TimeLastLineExit < lineTimes.TimeFirstLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");
                        if (r.TimeFirstLineExit > lineTimes.TimeLastLineExit)
                            throw new AlreadyExistException("The times of the line that has chose already have defination");
                    }

                }
            }

            XElement newLineElem = new XElement("LineTrip"
            , new XElement("BusLineIndentificator", lineTripToAdd.BusLineIndentificator),
            new XElement("TimeFirstLineExit", lineTripToAdd.TimeFirstLineExit.ToString()),
            new XElement("Frequency", lineTripToAdd.Frequency.ToString()),
            new XElement("TimeLastLineExit", lineTripToAdd.TimeLastLineExit.ToString())
            );
            linesTripRoot.Add(newLineElem);
            XMLTools.SaveListToXMLElement(linesTripRoot, linesTripPath);
        }

        public void DeleteLineTrip(int lineIdentificator, TimeSpan firstLineExit)
        {
            XElement linesTripRoot = XMLTools.LoadListFromXMLElement(linesTripPath);

            XElement myLineTimes = (from lineTripElem in linesTripRoot.Elements()
                                    where (lineTripElem.Element("BusLineIndentificator").Value == lineIdentificator.ToString()
                                    && lineTripElem.Element("TimeFirstLineExit").Value == firstLineExit.ToString())
                                    select lineTripElem).FirstOrDefault();
            if (myLineTimes == null)
                throw new DoesntExistException("the data about line doesn't exist in system");
            myLineTimes.Remove();
            XMLTools.SaveListToXMLElement(linesTripRoot, linesTripPath);
        }

        public LineTrip GetALineTimes(int lineIdentificator, TimeSpan firstLineExit)
        {
            XElement linesTripRoot = XMLTools.LoadListFromXMLElement(linesTripPath);

            XElement myLineTimes = (from lineTripElem in linesTripRoot.Elements()
                                    where (lineTripElem.Element("BusLineIndentificator").Value == lineIdentificator.ToString()
                                    && lineTripElem.Element("TimeFirstLineExit").Value == firstLineExit.ToString())
                                    select lineTripElem).FirstOrDefault();
            if (myLineTimes != null)
                return new LineTrip
                {
                    BusLineIndentificator = Convert.ToInt32(myLineTimes.Element("BusLineIndentificator").Value),
                    Frequency = TimeSpan.Parse(myLineTimes.Element("Frequency").Value),
                    TimeFirstLineExit = TimeSpan.Parse(myLineTimes.Element("TimeFirstLineExit").Value),
                    TimeLastLineExit = TimeSpan.Parse(myLineTimes.Element("TimeLastLineExit").Value)
                };

            throw new DoesntExistException("the data about line doesn't exists in system");
        }
        public IEnumerable<LineTrip> GetAllLinesTimes()
        {
            XElement linesTripRoot = XMLTools.LoadListFromXMLElement(linesTripPath);
            return from lineTrip in linesTripRoot.Elements()
                   select new LineTrip
                   {
                       BusLineIndentificator = Convert.ToInt32(lineTrip.Element("BusLineIndentificator").Value),
                       Frequency = TimeSpan.Parse(lineTrip.Element("Frequency").Value),
                       TimeFirstLineExit = TimeSpan.Parse(lineTrip.Element("TimeFirstLineExit").Value),
                       TimeLastLineExit = TimeSpan.Parse(lineTrip.Element("TimeLastLineExit").Value)
                   };
        }

        public IEnumerable<LineTrip> GetPartOfLinesTimes(Predicate<LineTrip> lineTimesCondition)
        {
            XElement linesTripRoot = XMLTools.LoadListFromXMLElement(linesTripPath);
            return from lineTrip in linesTripRoot.Elements()
                   let lineTripDO = new LineTrip
                   {
                       BusLineIndentificator = Convert.ToInt32(lineTrip.Element("BusLineIndentificator").Value),
                       Frequency = TimeSpan.Parse(lineTrip.Element("Frequency").Value),
                       TimeFirstLineExit = TimeSpan.Parse(lineTrip.Element("TimeFirstLineExit").Value),
                       TimeLastLineExit = TimeSpan.Parse(lineTrip.Element("TimeLastLineExit").Value)
                   }
                   where lineTimesCondition(lineTripDO)
                   select lineTripDO;

        }


        public void UpdateLineTrip(LineTrip lineTriptoUpdate, TimeSpan OldFirstExit)
        {
            XElement linesTripRoot = XMLTools.LoadListFromXMLElement(linesTripPath);
            var mylineTimes = (from lineTripElem in linesTripRoot.Elements()
                               where (lineTripElem.Element("BusLineIndentificator").Value == lineTriptoUpdate.BusLineIndentificator.ToString()
                               && lineTripElem.Element("TimeFirstLineExit").Value == OldFirstExit.ToString())
                               select lineTripElem).FirstOrDefault();

            if (mylineTimes == null)
                throw new DoesntExistException("This line doesn't exist in the system");

            var listOfOutherTimesOfLine = from x in GetAllLinesTimes()
                                          where x.BusLineIndentificator == lineTriptoUpdate.BusLineIndentificator
                                          && x.TimeFirstLineExit != OldFirstExit
                                          select x;

            //check if there is a bus exit that collision with the new times:
            foreach (var r in listOfOutherTimesOfLine)
            {
                //must be collision.. both drive in 00:00:00..
                if (lineTriptoUpdate.TimeFirstLineExit > lineTriptoUpdate.TimeLastLineExit && r.TimeFirstLineExit > r.TimeLastLineExit)
                    throw new AlreadyExistException("The times of the line that has chose already have defination");
                //both drive in time 00:00:00- 23:59:59
                if (lineTriptoUpdate.TimeFirstLineExit < lineTriptoUpdate.TimeLastLineExit && r.TimeFirstLineExit < r.TimeLastLineExit)
                {
                    if (lineTriptoUpdate.TimeFirstLineExit >= r.TimeFirstLineExit && lineTriptoUpdate.TimeFirstLineExit < r.TimeLastLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                    if (lineTriptoUpdate.TimeLastLineExit > r.TimeFirstLineExit && lineTriptoUpdate.TimeLastLineExit < r.TimeLastLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                    if (lineTriptoUpdate.TimeFirstLineExit < r.TimeFirstLineExit && lineTriptoUpdate.TimeLastLineExit > r.TimeLastLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                }
                //r drive from "day" to other "day" and lineTimes in one day..
                else if (r.TimeFirstLineExit > r.TimeLastLineExit && lineTriptoUpdate.TimeFirstLineExit < lineTriptoUpdate.TimeLastLineExit)
                {
                    if (r.TimeLastLineExit > lineTriptoUpdate.TimeFirstLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                    if (r.TimeFirstLineExit < lineTriptoUpdate.TimeLastLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");

                    //if (r.TimeFirstLineExit < lineTriptoUpdate.TimeFirstLineExit || r.TimeLastLineExit > lineTriptoUpdate.TimeLastLineExit)
                    //    throw new AlreadyExistException("The times of the line that has chose already have defination");
                }
                // lineTimes drive from "day" to other "day" and r in one day..
                else if (r.TimeFirstLineExit < r.TimeLastLineExit && lineTriptoUpdate.TimeFirstLineExit > lineTriptoUpdate.TimeLastLineExit)
                {
                    if (r.TimeLastLineExit < lineTriptoUpdate.TimeFirstLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                    if (r.TimeFirstLineExit > lineTriptoUpdate.TimeLastLineExit)
                        throw new AlreadyExistException("The times of the line that has chose already have defination");
                }
            }


            mylineTimes.Element("TimeFirstLineExit").Value = lineTriptoUpdate.TimeFirstLineExit.ToString();
            mylineTimes.Element("TimeLastLineExit").Value = lineTriptoUpdate.TimeLastLineExit.ToString();
            mylineTimes.Element("Frequency").Value = lineTriptoUpdate.Frequency.ToString();
            XMLTools.SaveListToXMLElement(linesTripRoot, linesTripPath);
        }
        #endregion

        #region stations
        public void AddStation(Station stationToAdd)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElement = (from p in stationRoot.Elements()
                                  where Convert.ToInt32(p.Element("StationCode").Value) == stationToAdd.StationCode
                                  select p).FirstOrDefault();
            if (stationElement != null)
                throw new AlreadyExistException("The station already exist in the system");

            XElement stationElem = new XElement("Station", new XElement("StationCode", stationToAdd.StationCode),
                                   new XElement("LocationLatitude", stationToAdd.LocationLatitude),
                                   new XElement("LocationLongitude", stationToAdd.LocationLongitude),
                                   new XElement("StationName", stationToAdd.StationName),
                                   new XElement("StationAddress", stationToAdd.StationAddress),
                                   new XElement("PlaceToSit", stationToAdd.PlaceToSit),
                                   new XElement("BoardBusTiming", stationToAdd.BoardBusTiming));
            stationRoot.Add(stationElem);
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);

        }

        public void DeleteStation(int stationCodeToDelete)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("StationCode").Value) == stationCodeToDelete
                                       select p).FirstOrDefault();
            if (stationElement == null)
                throw new DoesntExistException("This station doesn't exist in system");
            stationElement.Remove();
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }

        public Station GetAStation(int stationCode)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var station = (from p in stationRoot.Elements()
                           where p.Element("StationCode").Value == stationCode.ToString()
                           select p).FirstOrDefault();

            if (station != null)
                return new Station()
                {
                    StationCode = Convert.ToInt32(station.Element("StationCode").Value),
                    StationName = station.Element("StationName").Value,
                    LocationLatitude = Convert.ToDouble(station.Element("LocationLatitude").Value),
                    LocationLongitude = Convert.ToDouble(station.Element("LocationLongitude").Value),
                    StationAddress = station.Element("StationAddress").Value,
                    PlaceToSit = Convert.ToBoolean(station.Element("PlaceToSit").Value),
                    BoardBusTiming = Convert.ToBoolean(station.Element("BoardBusTiming").Value)
                };
            throw new DoesntExistException("The station doesn't exist in system");

        }

        public void UpdateStation(Station stationToUpdate)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("StationCode").Value) == stationToUpdate.StationCode
                                       select p).FirstOrDefault();
            stationElement.Element("StationName").Value = stationToUpdate.StationName;
            stationElement.Element("LocationLatitude").Value = stationToUpdate.LocationLatitude.ToString();
            stationElement.Element("LocationLongitude").Value = stationToUpdate.LocationLongitude.ToString();
            stationElement.Element("StationAddress").Value = stationToUpdate.StationAddress;
            stationElement.Element("PlaceToSit").Value = stationToUpdate.PlaceToSit.ToString();
            stationElement.Element("BoardBusTiming").Value = stationToUpdate.BoardBusTiming.ToString();

            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
        public IEnumerable<Station> GetPartOfStations(Predicate<Station> StationCondition)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            return from p in stationRoot.Elements()
                   let pAsStation = new Station()
                   {
                       StationCode = Convert.ToInt32(p.Element("StationCode").Value),
                       StationName = p.Element("StationName").Value,
                       LocationLatitude = Convert.ToDouble(p.Element("LocationLatitude").Value),
                       LocationLongitude = Convert.ToDouble(p.Element("LocationLongitude").Value),
                       StationAddress = p.Element("StationAddress").Value.ToString(),
                       PlaceToSit = Convert.ToBoolean(p.Element("PlaceToSit").Value),
                       BoardBusTiming = Convert.ToBoolean(p.Element("BoardBusTiming").Value)
                   }
                   where StationCondition(pAsStation)
                   select pAsStation;
        }

        public IEnumerable<Station> GetAllStations()
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            return from p in stationRoot.Elements()
                   select new Station()
                   {
                       StationCode = Convert.ToInt32(p.Element("StationCode").Value),
                       StationName = p.Element("StationName").Value,
                       LocationLatitude = Convert.ToDouble(p.Element("LocationLatitude").Value),
                       LocationLongitude = Convert.ToDouble(p.Element("LocationLongitude").Value),
                       StationAddress = p.Element("StationAddress").Value.ToString(),
                       PlaceToSit = Convert.ToBoolean(p.Element("PlaceToSit").Value),
                       BoardBusTiming = Convert.ToBoolean(p.Element("BoardBusTiming").Value)
                   };

        }
        #endregion

        public int GetTheLastBusIdentificator()
        {
            XElement configurationRoot = XMLTools.LoadListFromXMLElement(configurationPath);
            return (Convert.ToInt32(configurationRoot.Element("BusLineID").Value) - 1);
        }
 
    }
}
 
