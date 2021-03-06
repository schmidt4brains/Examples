﻿# SharpShooterReportsRunner.exe

This console app can be used to load data from an AQTS system or external sources and render the report using a SharpShooter Reports template.

This utility is intended to help AQUARIUS Time-Series 3.x customers convert their custom reports from the 3.X code base to work on the 20xx codebase of AQTS.

## Features

- Can be easily scripted, run on a schedule.
- Reports a non-zero exit code if something goes wrong.
- Logs its activity to the standard `%ProgramData%\Aquatic Informatics\AQUARIUS\Logs` folder in the `SharpShooterReportsRunner.log` file.
- Can upload a rendered PDF as an external report to AQTS.
- Can launch the SharpShooter Reports designer to edit the template in context with your AQTS data.

## Requirements

- .NET 4.7 runtime
- Credentials for an AQTS 20xx app server
- When run directly from an AQTS app server, no extra SharpShooter Reports license is required.
- A valid SharpShooter Reports license for 7.1.1.18 or greater is required if this code is run from a client computer connecting to an AQTS app server over the network. Contact support@aquaticinformatics.com to see if your account is eligible for a free license.

## Operation

- Run `SharpShooterReportsRunner /help` to see all the command line options.
- Add as many `TimeSeries=identifier` options as needed. Each time-series will be loaded into a separate dataset named `TimeSeriesX`, with `X` starting at 1.
- Add the `/LaunchReportDesigner=true` option to launch the SharpShooter Report designer GUI along with your time-series data.
- Your report may require many command-line options. Consider using the `@optionsFile` syntax to store options in a text file, one line per option. See the [wiki](https://github.com/AquaticInformatics/examples/wiki/Common-command-line-options) for details.

## Help screen
```
usage: SharpShooterReportsRunner [-option=value] [@optionsFile] ...

Supported -option=value settings (/option=value works too):

  ========================= AQUARIUS Time-Series connection options:
  -Server                   The AQTS app server from which time-series data will be retrieved.
  -Username                 AQTS username. [default: admin]
  -Password                 AQTS credentials. [default: admin]

  ========================= SharpShooter Reports options:
  -TemplatePath             Path of the SharpShooter Reports template (*.RST) or Aquarius Report template (*.ART) file.
  -OutputPath               Path to the generated report output. Only PDF output is supported.
  -LaunchReportDesigner     When true, launch the SharpShooter Report Designer. [default: False]

  ========================= Dataset options:
  -QueryFrom                The starting point for all time-series. Can be overriden by individual series. [default: Beginning of record]
  -QueryTo                  The ending point for all time-series. Can be overriden by individual series. [default: End of record]
  -GroupBy                  The grouping for all time-series. One of None, Day, Week, Month, Year, Decade. Can be overriden by individual series. [default: Year]
  -TimeSeries               Load the specified time-series as a dataset.
  -RatingModel              Load the specified rating-model as a dataset.
  -ExternalDataSet          Load the external DataSet XML file.

  ========================= Report uploading options:
  -UploadedReportLocation   Upload the generated report to this AQTS location identifier. If empty, no report will be uploaded.
  -UploadedReportTitle      Upload the generated report with this title. Defaults to the -OutputPath base filename.

Retrieving time-series data from AQTS: (more than one -TimeSeries=value option can be specified)

  -TimeSeries=identifierOrUniqueId[,From=date][,To=date][,Unit=outputUnit][,GroupBy=option][,DataSetName=dataSetName]

     =identifierOrUniqueId     - Use either the uniqueId or the <parameter>.<label>@<location> syntax.
     ,From=date                - Retrieve data from this date. [default: Beginning of record]
     ,To=date                  - Retrieve data until this date. [default: End of record]
     ,Unit=outputUnit          - Convert the values to the unit. [default: The default unit of the time-series]
     ,GroupBy=option           - Groups data by None|Day|Week|Month|Year|Decade [default: Year]
     ,DataSetName=datasetName  - Override the name of the dataset. [default: 'TimeSeries#' where # is the 1-based index of the time-series]

  Use the -TimeSeries4=... or -TimeSeries[4]=... syntax to force the dataset name to a specific index.

  Dates specified as yyyy-MM-ddThh:mm:ss.fff. Only the year component is required.

Retrieving rating model info from AQTS: (more than one -RatingModel=value option can be specified)

  -RatingModel=identifierOrUniqueId[,From=date][,To=date][,Unit=outputUnit][,GroupBy=option][,DataSetName=dataSetName]

     =identifierOrUniqueId     - Use either the uniqueId or the <InputParameter>-<OutputParameter>.<label>@<location> syntax.
     ,From=date                - Retrieve data from this date. [default: Beginning of record]
     ,To=date                  - Retrieve data until this date. [default: End of record]
     ,StepSize=increment       - Set the expanded table step size. [default: 0.1]
     ,DataSetName=datasetName  - Override the name of the dataset. [default: 'RatingCurve#' where # is the 1-based index of the rating model]

  Use the -RatingModel4=... or -RatingModel[4]=... syntax to force the dataset name to a specific index.

  Dates specified as yyyy-MM-ddThh:mm:ss.fff. Only the year component is required.

Using external data sets: (more than one -ExternalDataSet=value option can be specified)

  -ExternalDataSet=pathToXml[,DataSetName=dataSetName]

     =pathToXml                - A standard .NET DataSet, serialized to XML.
     ,DataSetName=datasetName  - Override the name of the dataset. [default: The name stored within the XML]

Unknown -name=value options will be merged with the appropriate data set and table.

  Simple -name=value options like -MySetting=MyValue will be added to the Common.CommandLineParameters table.

  Dotted -name=value options like -ReportParameters.Parameters.Description=MyValue will be merged into the named dataset.table.column.

Use the @optionsFile syntax to read more options from a file.

  Each line in the file is treated as a command line option.
  Blank lines and leading/trailing whitespace is ignored.
  Comment lines begin with a # or // marker.
```