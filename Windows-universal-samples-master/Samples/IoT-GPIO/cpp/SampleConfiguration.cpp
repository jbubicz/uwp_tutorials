//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

#include "pch.h"
#include "MainPage.xaml.h"
#include "SampleConfiguration.h"


Platform::Array<SDKTemplate::Scenario>^ SDKTemplate::MainPage::scenariosInner = ref new Platform::Array<SDKTemplate::Scenario>
{
    { "Get and set GPIO pin value", "SDKTemplate.Scenario1_GetAndSetPin" },
    { "Listen for changes in GPIO pin value", "SDKTemplate.Scenario2_Listen" },
};

