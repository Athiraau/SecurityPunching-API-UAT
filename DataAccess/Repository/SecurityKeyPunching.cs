using Dapper.Oracle;
using Dapper;
using DataAccess.Context;
using Oracle.ManagedDataAccess.Types;
using DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dto;
using DataAccess.Dto.Request;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using System.Reflection.Emit;
using Dapper.Extensions.SQL;
using System.Runtime.ConstrainedExecution;

namespace DataAccess.Repository
{
    public class SecurityKeyPunching:ISecurityKeyPunching
    {
        private DapperContext _dapperContext;
        private DtoWrapper _dto;
        

        public SecurityKeyPunching(DapperContext context, DtoWrapper dto)
        {
            _dapperContext = context;
            _dto = dto;
            
        }

        public async Task<dynamic> GetSecurityData(string flag, string pageval, string parval)
        {
            OracleRefCursor result = null;
            var procedureName = "proc_security_get_detail";
            var parameters = new OracleDynamicParameters();
            parameters.Add("p_flag", flag, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("p_pageval", pageval, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("p_parval1", parval, OracleMappingType.NVarchar2, ParameterDirection.Input);

            parameters.Add("as_outresult", result, OracleMappingType.RefCursor, ParameterDirection.Output);


            parameters.BindByName = true;
            using var connection = _dapperContext.CreateConnection();
            var response = await connection.QueryAsync<dynamic>
                (procedureName, parameters, commandType: CommandType.StoredProcedure);

            return response;
        }

        public async Task<dynamic> PostSecurityData(SecPunchImgPostDto secPunchImgPostDto)
        {
           

            //------------------------------------------------------------------------
            OracleRefCursor result = null;

            var procedureName = "proc_security_post_detail";
            var parameters = new OracleDynamicParameters();
            
            parameters.Add("p_flag", secPunchImgPostDto.p_flag, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("p_indata", secPunchImgPostDto.p_indata, OracleMappingType.NVarchar2, ParameterDirection.Input);
            parameters.Add("p_as_outresult", result, OracleMappingType.RefCursor, ParameterDirection.Output);
           
            parameters.BindByName = true;
            using var connection = _dapperContext.CreateConnection();
          //  ----------------------------------------------------------

            var SqlA = "select t.m_time from m_sec_daily_attend t where  to_date(t.current_dt)=to_date(sysdate) and t.sec_code = '" + secPunchImgPostDto.secCode + "'";
            var MT = await connection.QuerySingleOrDefaultAsync(SqlA);  //Morning Time

            var SqlB = "select t.m_sec_photo from dms.m_sec_punch_photo t where  to_date(t.curr_date)=to_date(sysdate) and t.sec_code = '" + secPunchImgPostDto.secCode + "'";
            var MP = await connection.QuerySingleOrDefaultAsync(SqlB);  //Morning Photo

            var SqlAA = "select t.e_time from m_sec_daily_attend t where  to_date(t.current_dt)=to_date(sysdate) and t.sec_code = '" + secPunchImgPostDto.secCode + "'";
            var ET = await connection.QuerySingleOrDefaultAsync(SqlAA);  //Evening Time

            var SqlBB = "select t.e_sec_photo from dms.m_sec_punch_photo t where  to_date(t.curr_date)=to_date(sysdate) and t.sec_code = '" + secPunchImgPostDto.secCode + "'";
            var EP = await connection.QuerySingleOrDefaultAsync(SqlBB);  //Evening Photo


            var response = await connection.QueryAsync<dynamic>(procedureName, parameters, commandType: CommandType.StoredProcedure);

            //---------------IMAGE UPLOAD starts -------------------------------------------------------------    

            // default query to avoid 'sql' error
            var sql = "select y.*, y.rowid from M_SEC_DAILY_ATTEND y where to_date(y.current_dt)=to_date(sysdate) and y.SEC_CODE= '" + secPunchImgPostDto.secCode + "' ";
          
            String[] strlist = Convert.ToString(secPunchImgPostDto.p_indata).Split("~", StringSplitOptions.RemoveEmptyEntries);
            var ShId= strlist[2];
                      

            DateTime t1 = DateTime.Parse(DateTime.Now.TimeOfDay.ToString());  //Punchig Time (Live Lime)
            DateTime t2 = DateTime.Parse("12:00:00");


            int res = DateTime.Compare(t1, t2); //Comparing Time


            if (ShId=="4") //SHIFT 4
            {
                if (res < 0)   // less than12:00

                {
                    if (MT == null && MP == null)    // ENTRY PHOTO PUNCH
                    {
                        //updating photo in the table as per the timing
                        sql = "update dms.m_sec_punch_photo set M_SEC_PHOTO = :ph where SEC_CODE = '" + secPunchImgPostDto.secCode + "' and CURR_DATE = to_date(sysdate)";

                    }
                }
                else   //greater than 12:00
                {
                    if (ET == null && EP == null)    // EXIT PHOTO PUNCH
                    {
                        // updating photo in the table as per the timing
                        sql = "update dms.m_sec_punch_photo set E_SEC_PHOTO = :ph where SEC_CODE='" + secPunchImgPostDto.secCode + "' and CURR_DATE = to_date(sysdate)";
                    }
                }
            }
            else   //SHIFT 5
            {
                if (res > 0) // greater than 12:00
                {

                    if (MT == null && MP == null)   // ENTRY PHOTO PUNCH
                    {
                        sql = "update dms.m_sec_punch_photo set M_SEC_PHOTO = :ph where SEC_CODE = '" + secPunchImgPostDto.secCode + "' and CURR_DATE = to_date(sysdate)";
                    }
                }
                else   // less than 12:00 
                {
                    if (ET == null && EP == null)   // EXIT PHOTO PUNCH
                    {
                        sql = "update dms.m_sec_punch_photo set E_SEC_PHOTO = :ph where SEC_CODE = '" + secPunchImgPostDto.secCode + "' and CURR_DATE = to_date(sysdate-1)";
                    }
                }

            }

            connection.Open();
            OracleParameter[] prm = new OracleParameter[1];
            OracleCommand cmd = (OracleCommand)connection.CreateCommand();

            prm[0] = cmd.Parameters.Add("ph", OracleDbType.Blob, secPunchImgPostDto.secPhoto, ParameterDirection.Input);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            

            return response;

        }


    }
}
