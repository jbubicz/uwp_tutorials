// Copyright (c) Microsoft. All rights reserved.

#include "pch.h"
#include "MainPage.xaml.h"
#include "SampleConfiguration.h"

using namespace SDKTemplate;

Platform::Array<Scenario>^ MainPage::scenariosInner = ref new Platform::Array<Scenario>
{
    { "DataEvents", "SDKTemplate.Scenario1_DataEvents" },
    { "Polling", "SDKTemplate.Scenario2_Polling" }
};
