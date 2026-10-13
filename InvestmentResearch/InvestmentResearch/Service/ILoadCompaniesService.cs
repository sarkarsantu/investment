namespace InvestmentResearch.Service
{
    public interface ILoadCompaniesService
    {
        void LoadNSECompaniesFromDhan();
        void LoadBSECompaniesFromDhan();
        Task UpdateCompaniesFromDatabase(int top);
    }
}
