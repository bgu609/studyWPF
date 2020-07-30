using Caliburn.Micro;
using MySql.Data.MySqlClient;
using System.Dynamic;
using ThirdCaliburnApp.Models;
using static ThirdCaliburnApp.Commons;

namespace ThirdCaliburnApp.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHaveDisplayName
    {
        #region 속성
        BindableCollection<EmployeesModel> employees;
        public BindableCollection<EmployeesModel> Employees
        {
            get => employees;
            set
            {
                employees = value;
                NotifyOfPropertyChange(() => Employees);
            }
        }

        EmployeesModel seletedEmployees;
        public EmployeesModel SelectedEmployees
        {
            get => seletedEmployees;
            set
            {
                seletedEmployees = value;

                id = value.ID;
                empname = value.EmpName;
                salary = value.Salary;
                deptname = value.DeptName;
                destination = value.Destination;

                NotifyOfPropertyChange(() => SelectedEmployees);
                NotifyOfPropertyChange(() => ID);
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => Destination);
            }
        }

        int id;
        public int ID {
            get=>id;
            set
            {
                id = value;
                NotifyOfPropertyChange(() => ID);
            }
        }

        string empname;
        public string EmpName
        {
            get => empname;
            set
            {
                empname = value;
                NotifyOfPropertyChange(() => EmpName);
            }
        }

        decimal salary;
        public decimal Salary
        {
            get => salary;
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
            }
        }

        string deptname;
        public string DeptName
        {
            get => deptname;
            set
            {
                deptname = value;
                NotifyOfPropertyChange(() => DeptName);
            }
        }

        string destination;
        public string Destination
        {
            get => destination;
            set
            {
                destination = value;
                NotifyOfPropertyChange(() => Destination);
            }
        }

        #endregion

        #region 생성자

        public ShellViewModel()
        {

        }

        #endregion

        public void GetEmployees()
        {
            using (MySqlConnection conn = new MySqlConnection(CONNSTRING))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(EmployeesTBL.SELECT_EMPLOYEES, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                Employees = new BindableCollection<EmployeesModel>();
                while (reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        ID = (int)reader["id"],
                        EmpName = reader["empName"].ToString(),
                        Salary = (decimal)reader["salary"],
                        DeptName = reader["deptName"].ToString(),
                        Destination = reader["destination"].ToString()
                    };
                    Employees.Add(temp);
                }

            }
        }

        public bool CanSaveButton
        {
            get
            {
                return !((ID == 0) && string.IsNullOrEmpty(EmpName) && (Salary == 0) &&
                    string.IsNullOrEmpty(DeptName) && string.IsNullOrEmpty(Destination));
            }
        }

        public void SaveButton()
        {
            int resultsRow = 0;
            using (MySqlConnection conn = new MySqlConnection(CONNSTRING))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(EmployeesTBL.UPDATE_EMPLOYEES, conn);

                MySqlParameter paramID = new MySqlParameter("@id", MySqlDbType.Int32);
                MySqlParameter paramEmpName = new MySqlParameter("@EmpName", MySqlDbType.VarChar, 45);
                MySqlParameter paramSalary = new MySqlParameter("@Salary", MySqlDbType.Decimal);
                MySqlParameter paramDeptName = new MySqlParameter("@DeptName", MySqlDbType.VarChar, 45);
                MySqlParameter paramDestination = new MySqlParameter("@Destination", MySqlDbType.VarChar, 45);

                paramID.Value = ID;
                paramEmpName.Value = EmpName;
                paramSalary.Value = Salary;
                paramDeptName.Value = DeptName;
                paramDestination.Value = Destination;
                
                cmd.Parameters.Add(paramID);
                cmd.Parameters.Add(paramEmpName);
                cmd.Parameters.Add(paramSalary);
                cmd.Parameters.Add(paramDeptName);
                cmd.Parameters.Add(paramDestination);

                resultsRow = cmd.ExecuteNonQuery();
            }

            if (resultsRow > 0)
            {
                GetEmployees();
            }
        }

        public void NewButton()
        {
            ID = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = string.Empty;
            Destination = string.Empty;
        }
    }
}
