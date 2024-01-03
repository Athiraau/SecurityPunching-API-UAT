using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class DapperContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _password;
        private readonly string _securityKey;


        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _password = _configuration.GetConnectionString("Password");
            _securityKey = _configuration.GetConnectionString("Key");
            _connectionString = _configuration.GetConnectionString("OracleConnection") + _password;
        }

        public IDbConnection CreateConnection()
           => new OracleConnection(_connectionString);

    }
}
