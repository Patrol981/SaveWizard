using ConsoleWizard;
var app = new App();
// this is only nessesary, because Octokit won't work other way than blocking .Result() type
Task.Run(async () => { await app.Run(); }).Wait();