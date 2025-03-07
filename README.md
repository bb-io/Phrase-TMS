# Blackbird.io Phrase

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Phrase TMS is a software as a service platform designed to automate and streamline translating and localizing digital products, such as web or mobile apps, websites, marketing content, etc. for international markets.

## Before setting up

Before you can connect you need to make sure that:

- You have a Phrase TMS account on the instance you want to connect to.
- You have permission to create and modify Registered OAuth apps.
- In Phrase go to Settings > Registered OAuth apps and click _New_.
- Fill in any name and description. For Redirect URI fill in: `https://bridge.blackbird.io/api/AuthorizationCode` and click _Save_.
- Copy the _Client ID_ of the newly created OAuth app.

## Connecting

1. Navigate to apps and search for Phrase.
2. Click _Add Connection_.
3. Name your connection for future reference e.g. 'My Phrase connection'.
4. Fill in the _Client ID_ that you copied from Phrase.
5. Select the data center your Phrase instance is hosted on. You can select between the US and EU data centers.
6. Click _Authorize connection_.
7. Follow the instructions that Phrase gives you, authorizing Blackbird.io to act on your behalf.
8. When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

![1741020881890](image/README/1741020881890.png)

## Actions

### Analysis

- **Search job analyses** searches through all analyses that were created for a specific job
- **Search project analyses** searches through all analyses that were created for a specific project
- **Get analysis data** returns the full details of a specific analysis.
- **Create analyses** create one or multiple analyses for jobs in a given project
- **Download analysis file** download an analysis file in specified format, you can choose from CSV, JSON and LOG

### Clients

- **Search clients** searches for clients on your instance matching certain criteria
- **Get client** get information about a single client
- **Create client** creates a new client in your Phrase instance
- **Update client** updates the information related to a specific client
- **Delete client** deletes a client

### Custom fields

Custom project field actions allow you to get and set the value of fields that you have created in Phrase.
Custom fields are associated with a specific type, e.g. date, number, text, etc. In Blackbird it's important to be 'type-safe', that's why every type has its own action pair.

- **Get project date custom field value**
- **Get project numeric custom field value**
- **Get project single select custom field value**
- **Get project multi select custom field value**
- **Get project text custom field value**
- **Set project date custom field value**
- **Set project numeric custom field value**
- **Set project single select custom field value**
- **Set project text custom field value**

> If a Custom field is not present in a project, it will be added as part of the `Set` action.

### Glossary

- **Create glossary** creates a new glossary in your Phrase instance that you can then fill manually or through the _Import glossary_ action.
- **Export glossary** returns the glossary as a Blackbird interchangable file (standard TBX) that can be imported into any other app.
- **Import glossary** takes a glossary from any other Blackbird app and imports it into a specific Phrase glossary.

### Job

- **Search jobs** returns a list of jobs in the project based on specified parameters
- **Get job** get all job information for a specific job
- **Create job** create a new job from a file upload. This action only takes a single target language and will return a single job.
- **Create jobs** create jobs for multiple target languages. This action will return multiple jobs as its output.
- **Delete job** deletes jobs from a project
- **Update job** Update a job's global data
- **Pre-translate job** pre-translate a job in the project

The following actions download job files in either their original format or as a bilingual file.
- **Download target file**
- **Download original file**
- **Download bilingual file**

The following actions update a job's file (source or target) from their original format or from a bilingual file.
- **Upload bilingual file**
- **Upload job source file**
- **Upload job target file**

### Project

- **Search projects** search for projects matching the filters of the input parameters
- **Find project** given the same parameters as _Search projects_, only returns the first matching project
- **Get project** get global project data for a specific project
- **Create project** Create a new project. It has the following special inputs:
  - **Propagate Translations**: Propagate translations to lower workflows when the source updates
- **Create project from template** same as the previous action, however in this action you can use an existing project template
- **Add project target language** adds a target language to the project
- **Update project** updates a project with specified details
- **Delete project** delete the specified project
- **Download project original files** download all the original source files (in the jobs) of a project
- **Download project target files** download all the translated files (in the jobs) of a project
- **Assign providers from template** assigns providers to the project or specific jobs who were predefined on a certain template
- **Find project termbase** get the termbase linked to a project based on optional filters

### Project reference file

- **Search project reference files** searches through all project reference files
- **Create project reference files** add new project reference files. In case no file parts are sent, only 1 reference is created with the given note. Either at least one file must be sent or the note must be specified.
- **Download project reference file** download project reference file
- **Delete project reference file** delete a specific project reference file

### Quality assurance

- **Run auto LQA** runs Auto LQA for specified job parts or all jobs in a given workflow step
- **Download LQA assessment** downloads a single xlsx report based on specific job ID
- **Get LQA assessment** get a specific LQA assessment

### Quote

- **Get quote** gets information about a quote
- **Create quote** creates a new quote from a project
- **Delete quote** deletes a quote

### Translation

- **Delete all translations** deletes all translations by project ID for the given jobs

### Translation memory

- **Create translation memory** create a new translation memory
- **Get translation memory** get information about a specific translation memory
- **Import TMX file** imports a new TMX file into the translation memory
- **Export translation memory** exports the selected translation memory as either a TMX or an XLSX
- **Insert segment into memory** insert a new segment into the translation memory
- **Delete translation memory** deletes the entire translation memory

### User

- **Search users** search through all users active on this Phrase instance
- **Find user** given the search parameters, returns the first matching user
- **Get user** get user information by ID
- **Add user** adds a new user
- **Update user** updates a specific user
- **Delete user** deletes a specific user

## Events

### Project

- **On project created**
- **On project deleted**
- **On project due date changed**
- **On project metadata updated**
- **On shared project assigned**
- **On project status changed**
- **Find project termbase** get the termbase linked to a project based on optional filters

### Job

- **On job created**
- **On job deleted**
- **On continuous job updated**
- **On job assigned**
- **On job due date changed**
- **On job exported**
- **On job source updated**
- **On job status changed** If you are using checkpoints, please fill all optional inputs (including Project ID). It will work even without a specified Project ID, but using it will allow us to check if the job already has the specified status. The status "Completed by linguist" will also trigger the event if the optional status "Completed" is set.
- **On job target updated**
- **On job unexported**
- **On all jobs in workflow step reached status** Triggered when all jobs in a specific workflow step reach a specified status. Returns only jobs in the specified workflow step

### Template

- **On template created**
- **On template deleted**
- **On template updated**

### Analysis

- **On analysis created**

### Quality assurance

- **On LQA reports created** (Polling event)

## Example

![1695995715372](image/README/1695995715372.png)

This example shows one of many use cases. Here, whenever an article is published we fetch the missing translations and retrieve the article as an HTML file. Then we create a new Phrase project with the missing locales as the target languages and upload the article as jobs. Once the project status is changed to Completed, we download the translated HTML articles and push the translations back to the CMS.

## Analysis with other systems

Our app allows you to export an analysis file (via the **Download analysis file** action) and use it in your workflow whenever you need it. You can also specify the format in which you want to download the file (such as JSON, CSV, or log), and then import it into the service of your choice.

Here's an example to demonstrate this feature between the Phrase and XTRF apps:

![phrase-xtrf-1](image/README/phrase-xtrf-1.png)

![phrase-xtrf-2](image/README/phrase-xtrf-2.png)

![phrase-xtrf-2](image/README/phrase-xtrf-3.png)

## Missing features

Not all API endpoints are covered, let us know if you are missing features or if you see other improvements!

## Feedback

Feedback to our implementation of Phrase is always very welcome. Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
