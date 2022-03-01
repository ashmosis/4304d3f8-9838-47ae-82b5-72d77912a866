using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Refactoring.LegacyService.Positions.Constants;

namespace Refactoring.LegacyService.Candidates.Services
{
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(ConfigurationName = "LegacyApp.ICandidateTestService")]
    public interface ICandidateCreditService
    {
        [OperationContract(Action = "http://xxx.com/ICandidateTestService/GetCredit")]
        int GetCredit(string firstname, string surname, DateTime dateOfBirth);

        bool RequiresCreditCheck(string positionName);

        int GetAdjustedCredit(IApplicant candidate);
    }

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface ICandidateCreditServiceChannel : ICandidateCreditService, IClientChannel
    {}

    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public partial class CandidateCreditServiceClient : ClientBase<ICandidateCreditService>, ICandidateCreditService
    {
        private readonly ICandidateCreditServiceChannel _candidateTestServiceChannelImplementation;
        public CandidateCreditServiceClient() { }
        
        public CandidateCreditServiceClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        { }

        public CandidateCreditServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        { }

        public CandidateCreditServiceClient(Binding binding, EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        { }

        public CandidateCreditServiceClient(Binding binding, EndpointAddress remoteAddress,
            ICandidateCreditServiceChannel channel) :
            base(binding, remoteAddress)
        {
            _candidateTestServiceChannelImplementation = channel;
        }

        public int GetCredit(string firstname, string surname, DateTime dateOfBirth)
        {
            return _candidateTestServiceChannelImplementation?.GetCredit(firstname, surname, dateOfBirth) ?? base.Channel.GetCredit(firstname, surname, dateOfBirth);
        }

        public bool RequiresCreditCheck(string positionName)
        {
            return positionName == PositionNames.SecuritySpecialist || positionName == PositionNames.FeatureDeveloper;
        }

        public int GetAdjustedCredit(IApplicant candidate)
        {
            var baseCredit = GetCredit(candidate.Firstname, candidate.Surname, candidate.DateOfBirth);
            return candidate.Position?.Name == PositionNames.SecuritySpecialist ? baseCredit / 2 : baseCredit;
        }
    }
}
