using InvestmentResearch.Data;
using InvestmentResearch.Model;
using System.Data;

namespace InvestmentResearch.Repository;

public class CompanyData : ICompanyData
{
    private readonly IDALAccess _dalAccess;
    public CompanyData(IDALAccess dalAccess)
    {
        _dalAccess = dalAccess;
    }

    public List<CompanyResearch> GetCompanyData(int top)
    {
        List<CompanyResearch> companyResearches = new List<CompanyResearch>();
        string sql = "SELECT top " + top.ToString() + " * FROM tblCompanyResearch where ISNULL(IsCompleted,0) = 0 order by ISNULL(NoOfAttempts,0)";
        var getData = _dalAccess.GetDataByQuery(sql);
        if (getData == null || getData.Rows == null || getData.Rows.Count == 0)
        {
            return companyResearches;
        }
        
        foreach (DataRow company in getData.Rows)
        {
            var companyResearch = new CompanyResearch
            {
                ISIN = company["ISIN"]?.ToString(),
                Symbol = company["Symbol"]?.ToString(),
                CompanyName = company["CompanyName"]?.ToString(),
                IsCompleted = company["IsCompleted"] != DBNull.Value ? Convert.ToBoolean(company["IsCompleted"]) : (bool?)null,
                IsWatchList = company["IsWatchList"] != DBNull.Value ? Convert.ToBoolean(company["IsWatchList"]) : (bool?)null,
                CompanyEstablished = company["CompanyEstablished"] != DBNull.Value ? Convert.ToInt32(company["CompanyEstablished"]) : (int?)null,
                IsCompanyProfileService = company["IsCompanyProfileService"] != DBNull.Value ? Convert.ToBoolean(company["IsCompanyProfileService"]) : (bool?)null,
                Sector = company["Sector"]?.ToString(),
                MarketCap = company["MarketCap"] != DBNull.Value ? Convert.ToDecimal(company["MarketCap"]) : (decimal?)null,
                IsShareHoldingsService = company["IsShareHoldingsService"] != DBNull.Value ? Convert.ToBoolean(company["IsShareHoldingsService"]) : (bool?)null,
                PromotersShare = company["PromotersShare"] != DBNull.Value ? Convert.ToDecimal(company["PromotersShare"]) : (decimal?)null,
                IsIncomeStatementService = company["IsIncomeStatementService"] != DBNull.Value ? Convert.ToBoolean(company["IsIncomeStatementService"]) : (bool?)null,
                ProfitAfterTax1 = company["ProfitAfterTax1"] != DBNull.Value ? Convert.ToDecimal(company["ProfitAfterTax1"]) : (decimal?)null,
                ProfitAfterTax2 = company["ProfitAfterTax2"] != DBNull.Value ? Convert.ToDecimal(company["ProfitAfterTax2"]) : (decimal?)null,
                ProfitAfterTax3 = company["ProfitAfterTax3"] != DBNull.Value ? Convert.ToDecimal(company["ProfitAfterTax3"]) : (decimal?)null,
                ProfitAfterTax4 = company["ProfitAfterTax4"] != DBNull.Value ? Convert.ToDecimal(company["ProfitAfterTax4"]) : (decimal?)null,
                ProfitAfterTax5 = company["ProfitAfterTax5"] != DBNull.Value ? Convert.ToDecimal(company["ProfitAfterTax5"]) : (decimal?)null,
                TotalRevenue1 = company["TotalRevenue1"] != DBNull.Value ? Convert.ToDecimal(company["TotalRevenue1"]) : (decimal?)null,
                TotalRevenue2 = company["TotalRevenue2"] != DBNull.Value ? Convert.ToDecimal(company["TotalRevenue2"]) : (decimal?)null,
                TotalRevenue3 = company["TotalRevenue3"] != DBNull.Value ? Convert.ToDecimal(company["TotalRevenue3"]) : (decimal?)null,
                TotalRevenue4 = company["TotalRevenue4"] != DBNull.Value ? Convert.ToDecimal(company["TotalRevenue4"]) : (decimal?)null,
                TotalRevenue5 = company["TotalRevenue5"] != DBNull.Value ? Convert.ToDecimal(company["TotalRevenue5"]) : (decimal?)null,
                CreateDate = company["CreateDate"] != DBNull.Value ? Convert.ToDateTime(company["CreateDate"]) : (DateTime?)null,
                NoOfAttempts = company["NoOfAttempts"] != DBNull.Value ? Convert.ToInt32(company["NoOfAttempts"]) : (int?)null,
                NameFromOutsource = company["NameFromOutsource"] != DBNull.Value ? Convert.ToString(company["NameFromOutsource"]) : (string?)null,
            };

            companyResearches.Add(companyResearch);
        }

        return companyResearches;
    }

    public CompanyResearch GetCompanyDataByIsin(string isin)
    {
        string sql = "SELECT * FROM tblCompanyResearch WHERE ISIN = '" + isin + "'";

        var getData = _dalAccess.GetDataByQuery(sql);
        if (getData == null || getData.Rows == null || getData.Rows.Count == 0)
        {
            return null;
        }
        var companyResearch = new CompanyResearch
        {
            ISIN = getData.Rows[0]["ISIN"]?.ToString(),
            Symbol = getData.Rows[0]["Symbol"]?.ToString(),
            CompanyName = getData.Rows[0]["CompanyName"]?.ToString(),
            IsCompleted = getData.Rows[0]["IsCompleted"] != DBNull.Value ? Convert.ToBoolean(getData.Rows[0]["IsCompleted"]) : (bool?)null,
            IsWatchList = getData.Rows[0]["IsWatchList"] != DBNull.Value ? Convert.ToBoolean(getData.Rows[0]["IsWatchList"]) : (bool?)null,
            CompanyEstablished = getData.Rows[0]["CompanyEstablished"] != DBNull.Value ? Convert.ToInt32(getData.Rows[0]["CompanyEstablished"]) : (int?)null,
            IsCompanyProfileService = getData.Rows[0]["IsCompanyProfileService"] != DBNull.Value ? Convert.ToBoolean(getData.Rows[0]["IsCompanyProfileService"]) : (bool?)null,
            Sector = getData.Rows[0]["Sector"]?.ToString(),
            MarketCap = getData.Rows[0]["MarketCap"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["MarketCap"]) : (decimal?)null,
            IsShareHoldingsService = getData.Rows[0]["IsShareHoldingsService"] != DBNull.Value ? Convert.ToBoolean(getData.Rows[0]["IsShareHoldingsService"]) : (bool?)null,
            PromotersShare = getData.Rows[0]["PromotersShare"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["PromotersShare"]) : (decimal?)null,
            IsIncomeStatementService = getData.Rows[0]["IsIncomeStatementService"] != DBNull.Value ? Convert.ToBoolean(getData.Rows[0]["IsIncomeStatementService"]) : (bool?)null,
            ProfitAfterTax1 = getData.Rows[0]["ProfitAfterTax1"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["ProfitAfterTax1"]) : (decimal?)null,
            ProfitAfterTax2 = getData.Rows[0]["ProfitAfterTax2"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["ProfitAfterTax2"]) : (decimal?)null,
            ProfitAfterTax3 = getData.Rows[0]["ProfitAfterTax3"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["ProfitAfterTax3"]) : (decimal?)null,
            ProfitAfterTax4 = getData.Rows[0]["ProfitAfterTax4"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["ProfitAfterTax4"]) : (decimal?)null,
            ProfitAfterTax5 = getData.Rows[0]["ProfitAfterTax5"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["ProfitAfterTax5"]) : (decimal?)null,
            TotalRevenue1 = getData.Rows[0]["TotalRevenue1"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["TotalRevenue1"]) : (decimal?)null,
            TotalRevenue2 = getData.Rows[0]["TotalRevenue2"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["TotalRevenue2"]) : (decimal?)null,
            TotalRevenue3 = getData.Rows[0]["TotalRevenue3"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["TotalRevenue3"]) : (decimal?)null,
            TotalRevenue4 = getData.Rows[0]["TotalRevenue4"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["TotalRevenue4"]) : (decimal?)null,
            TotalRevenue5 = getData.Rows[0]["TotalRevenue5"] != DBNull.Value ? Convert.ToDecimal(getData.Rows[0]["TotalRevenue5"]) : (decimal?)null
        };

        return companyResearch;
    }

    public CompanyResearch InsertCompanyDate(CompanyResearch companyResearch)
    {
        string columns = string.Empty, value = string.Empty;
        if (companyResearch != null)
        {
            if (companyResearch.CompanyEstablished != null && companyResearch.CompanyEstablished > 0)
            {
                columns += ", CompanyEstablished";
                value += $", {companyResearch.CompanyEstablished}";
            }
            if (companyResearch.IsCompanyProfileService != null && companyResearch.IsCompanyProfileService == true)
            {
                columns += ", IsCompanyProfileService, Sector, MarketCap";
                value += ",'" + companyResearch.IsCompanyProfileService + "'";
                value += companyResearch.Sector != null ? ",'" + companyResearch.Sector + "'" : ",NULL";
                value += companyResearch.MarketCap != null ? ",'" + companyResearch.MarketCap + "'" : ",NULL";
            }
            if (companyResearch.IsShareHoldingsService != null && companyResearch.IsShareHoldingsService == true)
            {
                columns += ", IsShareHoldingsService, PromotersShare";
                value += ",'" + companyResearch.IsShareHoldingsService + "'";
                value += companyResearch.PromotersShare != null ? ",'" + companyResearch.PromotersShare + "'" : "NULL";
            }
            if (companyResearch.IsIncomeStatementService != null && companyResearch.IsIncomeStatementService == true)
            {
                columns += ", IsIncomeStatementService, ProfitAfterTax1, ProfitAfterTax2, ProfitAfterTax3, ProfitAfterTax4, ProfitAfterTax5, ProfitAfterTaxCagr, TotalRevenue1, TotalRevenue2, TotalRevenue3, TotalRevenue4, TotalRevenue5,TotalRevenueCagr";
                value += ",'" + companyResearch.IsIncomeStatementService + "'";
                value += companyResearch.ProfitAfterTax1 != null ? ",'" + companyResearch.ProfitAfterTax1 + "'" : ",NULL";
                value += companyResearch.ProfitAfterTax2 != null ? ",'" + companyResearch.ProfitAfterTax2 + "'" : ",NULL";
                value += companyResearch.ProfitAfterTax3 != null ? ",'" + companyResearch.ProfitAfterTax3 + "'" : ",NULL";
                value += companyResearch.ProfitAfterTax4 != null ? ",'" + companyResearch.ProfitAfterTax4 + "'" : ",NULL";
                value += companyResearch.ProfitAfterTax5 != null ? ",'" + companyResearch.ProfitAfterTax5 + "'" : ",NULL";
                value += companyResearch.ProfitAfterTaxCagr != null ? ",'" + companyResearch.ProfitAfterTaxCagr + "'" : ",NULL";
                value += companyResearch.TotalRevenue1 != null ? ",'" + companyResearch.TotalRevenue1 + "'" : ",NULL";
                value += companyResearch.TotalRevenue2 != null ? ",'" + companyResearch.TotalRevenue2 + "'" : ",NULL";
                value += companyResearch.TotalRevenue3 != null ? ",'" + companyResearch.TotalRevenue3 + "'" : ",NULL";
                value += companyResearch.TotalRevenue4 != null ? ",'" + companyResearch.TotalRevenue4 + "'" : ",NULL";
                value += companyResearch.TotalRevenue5 != null ? ",'" + companyResearch.TotalRevenue5 + "'" : ",NULL";
                value += companyResearch.TotalRevenueCagr != null ? ",'" + companyResearch.TotalRevenueCagr + "'" : ",NULL";
            }
            if (!string.IsNullOrWhiteSpace(companyResearch.Reason))
            {
                columns += ", Reason";
                value += ",'" + companyResearch.Reason + "'";
            }
            columns += ", CreateDate,NoOfAttempts";
            value += ",'" + companyResearch.CreateDate + "'," + companyResearch.NoOfAttempts;
        }
        string sql = $"INSERT INTO tblCompanyResearch (ISIN, Symbol, CompanyName, IsCompleted {columns}) " +
                     $"VALUES ('{companyResearch.ISIN}', '{companyResearch.Symbol}', '{companyResearch.CompanyName}', '{companyResearch.IsCompleted}' {value})";

        _dalAccess.SaveDataByQuery(sql);
        return companyResearch;
    }

    public CompanyResearch UpdateCompanyData(CompanyResearch companyResearch)
    {
        string sql = $"UPDATE tblCompanyResearch SET";
        if (companyResearch.ISIN != null)
            sql += $" ISIN = '{companyResearch.ISIN}'";
        if (companyResearch.Symbol != null)
            sql += $", Symbol = '{companyResearch.Symbol}'";
        if (companyResearch.CompanyName != null)
            sql += $", CompanyName = '{companyResearch.CompanyName}'";
        if (companyResearch.IsCompleted != null)
            sql += $", IsCompleted = '{companyResearch.IsCompleted}'";
        if (companyResearch.Reason != null)
            sql += $", Reason = '{companyResearch.Reason}'";
        if (companyResearch.CompanyEstablished != null)
            sql += $", CompanyEstablished = '{companyResearch.CompanyEstablished}'";
        if (companyResearch.ModifyDate != null)
            sql += $", ModifyDate = '{companyResearch.ModifyDate}'";

        if (companyResearch.IsCompanyProfileService != null && companyResearch.IsCompanyProfileService == true)
        {
            sql += $", IsCompanyProfileService = '{companyResearch.IsCompanyProfileService}'";
            if (companyResearch.Sector != null)
                sql += $", Sector = '{companyResearch.Sector}'";
            if (companyResearch.MarketCap != null)
                sql += $", MarketCap = {companyResearch.MarketCap}";
        }
        if (companyResearch.IsShareHoldingsService != null && companyResearch.IsShareHoldingsService == true)
        {
            sql += $", IsShareHoldingsService = '{companyResearch.IsShareHoldingsService}'";
            if (companyResearch.PromotersShare != null)
                sql += $", PromotersShare = {companyResearch.PromotersShare}";
        }
        if (companyResearch.IsIncomeStatementService != null && companyResearch.IsIncomeStatementService == true)
        {
            sql += $", IsIncomeStatementService = '{companyResearch.IsIncomeStatementService}'";
            if (companyResearch.ProfitAfterTax1 != null)
                sql += $", ProfitAfterTax1 = {companyResearch.ProfitAfterTax1}";
            if (companyResearch.ProfitAfterTax2 != null)
                sql += $", ProfitAfterTax2 = {companyResearch.ProfitAfterTax2}";
            if (companyResearch.ProfitAfterTax3 != null)
                sql += $", ProfitAfterTax3 = {companyResearch.ProfitAfterTax3}";
            if (companyResearch.ProfitAfterTax4 != null)
                sql += $", ProfitAfterTax4 = {companyResearch.ProfitAfterTax4}";
            if (companyResearch.ProfitAfterTax5 != null)
                sql += $", ProfitAfterTax5 = {companyResearch.ProfitAfterTax5}";
            if (companyResearch.ProfitAfterTaxCagr != null)
                sql += $", ProfitAfterTaxCagr = {companyResearch.ProfitAfterTaxCagr}";
            if (companyResearch.TotalRevenue1 != null)
                sql += $", TotalRevenue1 = {companyResearch.TotalRevenue1}";
            if (companyResearch.TotalRevenue2 != null)
                sql += $", TotalRevenue2 = {companyResearch.TotalRevenue2}";
            if (companyResearch.TotalRevenue3 != null)
                sql += $", TotalRevenue3 = {companyResearch.TotalRevenue3}";
            if (companyResearch.TotalRevenue4 != null)
                sql += $", TotalRevenue4 = {companyResearch.TotalRevenue4}";
            if (companyResearch.TotalRevenue5 != null)
                sql += $", TotalRevenue5 = {companyResearch.TotalRevenue5}";
            if (companyResearch.TotalRevenueCagr != null)
                sql += $", TotalRevenueCagr = {companyResearch.TotalRevenueCagr}";
        }
        sql += $", NoOfAttempts = {companyResearch.NoOfAttempts}";
        sql += " where ISIN = '" + companyResearch.ISIN + "'";

        _dalAccess.SaveDataByQuery(sql);
        return companyResearch;
    }

    public int SaveCompanyDataFromOutsource(string NameFromOutsource)
    {
        if (NameFromOutsource.IndexOf("'") > 0)
            NameFromOutsource = NameFromOutsource.Replace("'", "''");
        string sql = "SELECT * FROM tblCompanyResearch WHERE NameFromOutsource = '" + NameFromOutsource.Trim() + "'";

        var getData = _dalAccess.GetDataByQuery(sql);
        if (getData == null || getData.Rows == null || getData.Rows.Count == 0)
        {
            sql = "INSERT INTO tblCompanyResearch (NameFromOutsource, CreateDate) VALUES ('" + NameFromOutsource + "','" + DateTime.Now + "')";
            return _dalAccess.SaveDataByQuery(sql);
        }

        return Convert.ToInt32(getData.Rows[0]["CompanyResearchID"]);
    }

    public CompanyResearch UpdateCompanyDataWithISIN(CompanyResearch companyResearch)
    {
        string sql = $"UPDATE tblCompanyResearch SET";
        if (companyResearch.ISIN != null)
            sql += $" ISIN = '{companyResearch.ISIN}',";
        if (companyResearch.CompanyName != null)
            sql += $" CompanyName = '{companyResearch.CompanyName}',";
        if (companyResearch.ModifyDate != null)
            sql += $" ModifyDate = '{companyResearch.ModifyDate}',";
        if (companyResearch.NoOfAttempts != null)
            sql += $" NoOfAttempts = {companyResearch.NoOfAttempts},";
        sql = sql.Substring(0, sql.Length - 1);

        sql += " where NameFromOutsource = '" + companyResearch.NameFromOutsource + "'";

        _dalAccess.SaveDataByQuery(sql);
        return companyResearch;
    }
}
