# Question

  The project `LegacyService` is a library written long time ago. Assuming the logic is perfectly **sound**, you are asked to refactor and modernise the code base, as well as improve its performance if possible.

  A couple of junior developers recently created a WebAPI `JuniorApi` that references `LegacyService`. They aren't very experienced and assume there is room for improvement. You are asked to supervise them.

  They also contributed in writting a consumer class in `Refactoring.ApiConsumer` project. Pretending this project belongs to a greater system, you shall only focus on helping your colleague to improve `RegisterNewCandidateConsumer` class.

## Tasks

### Refactor by applying clean code principles e.g. SOLID and design patterns for the following classes

- Please focus on refactoring `Refactoring.LegacyService.CandidateService`;
- Only if you have extra time, you can refactor `Refactoring.JuniorApi.Controllers.CandidatesController`, or leave your thoughts under **[Candidate-Comments](#Candidate-Comments)**;
- If above tasks cannot satisfy your appetite, consider refactor `Refactoring.ApiConsumer.Consumers.RegisterNewCandidateConsumer` as well, or leave your thoughts under **[Candidate-Comments](#Candidate-Comments)**.

### Complete at least one test in the following test projects

- Please focus on tests covering `Refactoring.LegacyService.CandidateService`, and create the tests in `Tests\Refactoring.LegacyService.Tests`;
- Only if you have extra time and working on refactoring `Refactoring.JuniorApi.Controllers.CandidatesController`, consider completing `Tests\Refactoring.JuniorApi.Tests`;
- Only if you have extra time and working on refactoring `Refactoring.ApiConsumer.Consumers.RegisterNewCandidateConsumer`, consider completing `Tests\Refactoring.ApiConsumer.Consumers.RegisterNewCandidateConsumer.Tests`;

## Restrictions

- Everything inside `Refactoring.ApiConsumer.NoChangeInfrastructure` namespace shall **NOT** BE CHNAGE AT ALL
- `CandidateDataAccess` class and its `AddCandidate` method need to stay **static**
- Feel free to use you prefered Mock/Assertion libraries in the test projects

## Candidate Comments

I had initially run out of time and was not happy with my implementation. In truth, I did not do well with the time aspect as normally I would take the time to think about the design first.

The new design separates concerns by delegating responsibilities. In this way the service becomes extremely streamlined, it's sole purpose is to retrieve a built candidate from the parameters and place it in the repo.

The builder does most of the heavy lifting. I had chosen a builder as it will ensure that we can't have a half-baked candidate that does not have the requisite checks completed before storing in the db. It also encapsulates the validation logic nicely.

I did move the responsibility of adjusting the credit score to the CandidateCreditService, although this could be added to the builder, the service already had access to the original credit score, Additionally, if I was looking for credit related methods, that would be where I would be looking.

I did have to make some changes in order to support dependency injection, both in the legacy service and in the web layer.

With regards to the JuniorApi; my recommendation would be the following:
1. Pull the validation out of the Controller, it does not belong there. The controller should be really simple; then the service and return the result.
2. The CandidateRequest should be in it's own file.
3. Validation can be delegated to something like [FluentValidation](https://fluentvalidation.net/) which means the validator would be in it's own class/file too. Alternatively, they can make use of the validation provided by [.Net Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1) by adding validation attributes to the model.
4. Without being too restrictive, we should really do a better job with validation. Names should have a max acceptable length and email address should be validated to a known format.
5. We should try not to re-throw the exception at the controller level, rather wwe should log the exception and try to handle known exception paths, but ultimately return a http response ourselves (ie. 404 for an unhandled exception).
6. Strings that are used, like in [Startup](./Refactoring.JuniorApi/Startup.cs) should be moved to a constants file.
