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

### Here is where candidates can leave comments*
