// GENERATED AUTOMATICALLY FROM 'Assets/Input/Actions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Actions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Actions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Actions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f6e1bc5c-ce79-482e-a1fd-310ddd859b59"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""422b7779-1260-47a0-9411-a71d8b959e7f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1e499781-509d-4a66-9743-1df092992713"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""398e4370-ba97-4ae5-ae25-e2610f298994"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""927dd6a1-de15-4dd2-8018-616e19cb0e3c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""7731b50d-7edf-4c4b-ad22-8bea43fd1079"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3030236e-e75f-4d68-9f6c-096f3ee066ff"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2a2c2ce6-d2d9-4ef6-b4d9-52862f2e8a98"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f1e75fa6-6b7f-4ce3-968f-8eb8ab2873f7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""54511ef3-08ad-4e17-8acb-154532c40e2b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Stick"",
                    ""id"": ""3cb24160-36dd-4098-9b45-e7876a767cf8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b8a67cda-9342-4735-8cfa-7e24e8152f67"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""36ab1d9c-28c3-47de-8fb4-f7d247101b9b"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""0632ae50-901f-4263-b92e-5fef4c921b3e"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0f172e84-7dd2-4947-ab83-a91a1fd549d0"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2a74474e-d33c-4c50-b7b2-7b5739172069"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""051a1640-c4b1-4aec-839b-5982279a94c0"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8e55a037-1e57-42f1-b4c3-5615691b8e5e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d3686b6-76ce-4a6c-9801-d3f495d599ec"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23d5d04c-aa83-43d2-9554-00d604708f3b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db6a2788-ddcf-4764-80ce-f6e55e001405"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""2bab97f1-b26e-4b4d-a057-77ae8fd84f07"",
            ""actions"": [
                {
                    ""name"": ""Selection Change"",
                    ""type"": ""PassThrough"",
                    ""id"": ""73ad625c-24af-4615-9c30-f74c4d79b233"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftJoystick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0f49247f-0d31-41af-a537-e7b4dfdc6ff6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightJoystick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c90c6134-3ac8-4ca7-9264-96bfaf18bd57"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Map"",
                    ""type"": ""Button"",
                    ""id"": ""99aee9de-de17-4acb-8c4c-746abaa86978"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Crafting"",
                    ""type"": ""Button"",
                    ""id"": ""037bc527-0756-4604-8af8-7f4e1aec7da2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""45a288c3-f035-4fa9-ada0-fba338364e59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""A"",
                    ""type"": ""Button"",
                    ""id"": ""f37c53cb-019d-44b2-b992-f7e12373acba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""X"",
                    ""type"": ""Button"",
                    ""id"": ""b40950f1-fc23-4600-bd51-611b7f645b0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""B"",
                    ""type"": ""Button"",
                    ""id"": ""74baa7c8-c802-4971-8531-d26e0ba98ec2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Y"",
                    ""type"": ""Button"",
                    ""id"": ""2a23dc54-7fb9-43ec-a75a-9c2b2f7c6f78"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""d7b16516-bd5e-4777-968d-d1797892724d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""a36ab08a-a456-49c4-9d52-e879d0198f7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""LB"",
                    ""type"": ""Button"",
                    ""id"": ""73135f96-fb79-4869-9465-be820cb57f3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""RB"",
                    ""type"": ""Button"",
                    ""id"": ""f825c761-529f-4089-891c-f4cae751cb7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""LT"",
                    ""type"": ""Button"",
                    ""id"": ""c233f7c5-bfaf-460c-9f45-6e445b1fbdb6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""RT"",
                    ""type"": ""Button"",
                    ""id"": ""d2afb073-b029-40c7-a331-ace487d429ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DUp"",
                    ""type"": ""Button"",
                    ""id"": ""393840c1-80b8-4d13-a840-68eaf4545a95"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DDown"",
                    ""type"": ""Button"",
                    ""id"": ""abfaaa92-5f7c-49d2-a755-a776aa21d9ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DLeft"",
                    ""type"": ""Button"",
                    ""id"": ""b63255cf-d342-4ab2-9841-a7747bfcf406"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""DRight"",
                    ""type"": ""Button"",
                    ""id"": ""1b11cf16-2e9d-4bfe-9402-ffffc9761846"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad D-Pad"",
                    ""id"": ""782963b0-5df3-47e5-bc26-00b1441c9096"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""dbc8b3b9-8e16-4196-a807-41d2cf8f9902"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""39884546-08b7-4ba7-a6e6-edd51e1e9e35"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""bd61f318-c6b6-4bc3-8503-e09433d85f9c"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""cb70dc0d-3291-401f-9624-8d22c54190d7"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Joystick"",
                    ""id"": ""0e845577-def0-4f68-a7e5-027f94bf9d77"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""f44c7cbb-8452-4be1-870c-59310a61c692"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""5dd49a5c-490e-4c04-af89-aca7d38771ba"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""c6014b5d-dec1-4c1a-9b70-f0bad0aa7716"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""bf3bfe95-e43d-4ff0-b56c-74a70d897169"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Arrows"",
                    ""id"": ""4f0d36d2-7482-4c2e-b0e9-8e16024643d5"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""7c2ec27a-c71b-47c2-8892-7dbd68b642d1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""b6a620da-154a-476b-885c-d68aba14806d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""cff9cf58-400a-45a0-8ebf-8abb2c327e18"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""76105637-e735-4436-b810-1230b6d3d547"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Selection Change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""b6fe5f39-6f3e-48a4-84c2-10a0d829aba1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cbeb8a4f-618a-43e2-bfb0-2c3691b7672b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f1016c69-4121-4daa-87e5-6419cf256c22"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6654d512-e589-4e9d-82d2-fc440ff3f2f5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dc1239ff-00cf-4c52-b308-6a167df00f42"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Stick"",
                    ""id"": ""289c0c7b-33bd-472b-affe-3a3a17b3e15b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0680833e-23e9-4fa3-907b-b8fdf0009a9e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""391e5dc1-4307-4d82-9f0b-b18d308a9608"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""801921e3-6ff2-41d5-a5c0-de142f001c09"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5a083e47-449e-48f1-9805-67f7abca79e8"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e0144557-76af-40a7-9cba-7ff6df373a4f"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3418a5d-20bc-4d9b-838c-e78727550744"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Crafting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa0fedcb-668b-4309-bf61-8e992d6450ae"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""411af6c6-ffa4-4167-b91f-5ed214b5f5a0"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2978a2fc-c6f3-46a5-8285-5d29306131ab"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""777513f4-792b-45bb-a161-12b4c07a03ea"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""B"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c67abd0a-797e-4b2f-8371-45c3ad5dc20a"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Y"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c623395-cf4b-4fbe-b166-973a84af560f"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13fab130-7a11-4d68-9622-63ccf38c16a5"",
                    ""path"": ""<Keyboard>/f10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb32f62b-bc00-4ea8-b782-a6c1705ac5f5"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ecef1dd5-4737-4644-90ee-5b8eecff8e37"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""228f4c78-723d-42f5-bea4-2583b7beaf71"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59b467ac-9f02-4de5-ae9d-d1f2aee7b854"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cdf6464e-593d-4564-a84d-cb7916dbac60"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1d38bf6-b126-4c27-aec8-2d55620b0474"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""X"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""989256fb-fab2-4a74-99d7-d71b758d941f"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25018f8c-970f-4fc7-b25d-8b151d02035f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""B"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d720873f-ca7e-4122-a6a4-d7bfa752bba9"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66359c4e-b178-4229-949b-5a22a18a41a2"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6aa5285-fcd7-4e8e-acb3-01349721a9b9"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""917e1ecb-e581-467c-bb76-a08df450690d"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""LT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f71675b-d810-47ec-9b66-5b11ad904096"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e72ee29-784a-404f-b152-8dc912434556"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18c76510-4d5b-4c5e-b537-da6c222a6e30"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0159778-c258-4f30-9eb0-ef029c9b8548"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5713cc57-3177-4610-a55d-153f81ae183c"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f24f7317-166c-41bc-a757-8a33fef42a76"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""156a7091-4528-46e2-a6fa-deb1bb9f7a12"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fd6a69e-f6d3-4e94-857a-10a6d7f9699f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""DRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""8bc7a2a5-4885-4157-956e-3f2b3f8da44a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""88b73c49-1008-4dd1-9599-75f31ccf2833"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""caf198c0-24cc-4ef5-a0cb-1a7cd79941c0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""32aa92ed-94d6-41be-9bbe-2c37ec6ab7b1"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ed5d3e21-60e8-4565-a12d-4df11ac37021"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Stick"",
                    ""id"": ""954f958d-1bd0-483e-a7cd-c788547a1e97"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""99b3340a-f5b1-45fb-bee1-d055baebf5b9"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""191495c0-bdde-4bc5-ae4c-8eacc29edcd3"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""215bed8b-295d-4436-b321-a4c2f4c0266f"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""42471c9c-7aed-416c-a74e-988057372c75"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Control Scheme Alpha"",
            ""bindingGroup"": ""Control Scheme Alpha"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_SelectionChange = m_Menu.FindAction("Selection Change", throwIfNotFound: true);
        m_Menu_LeftJoystick = m_Menu.FindAction("LeftJoystick", throwIfNotFound: true);
        m_Menu_RightJoystick = m_Menu.FindAction("RightJoystick", throwIfNotFound: true);
        m_Menu_Map = m_Menu.FindAction("Map", throwIfNotFound: true);
        m_Menu_Crafting = m_Menu.FindAction("Crafting", throwIfNotFound: true);
        m_Menu_Inventory = m_Menu.FindAction("Inventory", throwIfNotFound: true);
        m_Menu_A = m_Menu.FindAction("A", throwIfNotFound: true);
        m_Menu_X = m_Menu.FindAction("X", throwIfNotFound: true);
        m_Menu_B = m_Menu.FindAction("B", throwIfNotFound: true);
        m_Menu_Y = m_Menu.FindAction("Y", throwIfNotFound: true);
        m_Menu_Start = m_Menu.FindAction("Start", throwIfNotFound: true);
        m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
        m_Menu_LB = m_Menu.FindAction("LB", throwIfNotFound: true);
        m_Menu_RB = m_Menu.FindAction("RB", throwIfNotFound: true);
        m_Menu_LT = m_Menu.FindAction("LT", throwIfNotFound: true);
        m_Menu_RT = m_Menu.FindAction("RT", throwIfNotFound: true);
        m_Menu_DUp = m_Menu.FindAction("DUp", throwIfNotFound: true);
        m_Menu_DDown = m_Menu.FindAction("DDown", throwIfNotFound: true);
        m_Menu_DLeft = m_Menu.FindAction("DLeft", throwIfNotFound: true);
        m_Menu_DRight = m_Menu.FindAction("DRight", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Interact;
    public struct PlayerActions
    {
        private @Actions m_Wrapper;
        public PlayerActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_SelectionChange;
    private readonly InputAction m_Menu_LeftJoystick;
    private readonly InputAction m_Menu_RightJoystick;
    private readonly InputAction m_Menu_Map;
    private readonly InputAction m_Menu_Crafting;
    private readonly InputAction m_Menu_Inventory;
    private readonly InputAction m_Menu_A;
    private readonly InputAction m_Menu_X;
    private readonly InputAction m_Menu_B;
    private readonly InputAction m_Menu_Y;
    private readonly InputAction m_Menu_Start;
    private readonly InputAction m_Menu_Select;
    private readonly InputAction m_Menu_LB;
    private readonly InputAction m_Menu_RB;
    private readonly InputAction m_Menu_LT;
    private readonly InputAction m_Menu_RT;
    private readonly InputAction m_Menu_DUp;
    private readonly InputAction m_Menu_DDown;
    private readonly InputAction m_Menu_DLeft;
    private readonly InputAction m_Menu_DRight;
    public struct MenuActions
    {
        private @Actions m_Wrapper;
        public MenuActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectionChange => m_Wrapper.m_Menu_SelectionChange;
        public InputAction @LeftJoystick => m_Wrapper.m_Menu_LeftJoystick;
        public InputAction @RightJoystick => m_Wrapper.m_Menu_RightJoystick;
        public InputAction @Map => m_Wrapper.m_Menu_Map;
        public InputAction @Crafting => m_Wrapper.m_Menu_Crafting;
        public InputAction @Inventory => m_Wrapper.m_Menu_Inventory;
        public InputAction @A => m_Wrapper.m_Menu_A;
        public InputAction @X => m_Wrapper.m_Menu_X;
        public InputAction @B => m_Wrapper.m_Menu_B;
        public InputAction @Y => m_Wrapper.m_Menu_Y;
        public InputAction @Start => m_Wrapper.m_Menu_Start;
        public InputAction @Select => m_Wrapper.m_Menu_Select;
        public InputAction @LB => m_Wrapper.m_Menu_LB;
        public InputAction @RB => m_Wrapper.m_Menu_RB;
        public InputAction @LT => m_Wrapper.m_Menu_LT;
        public InputAction @RT => m_Wrapper.m_Menu_RT;
        public InputAction @DUp => m_Wrapper.m_Menu_DUp;
        public InputAction @DDown => m_Wrapper.m_Menu_DDown;
        public InputAction @DLeft => m_Wrapper.m_Menu_DLeft;
        public InputAction @DRight => m_Wrapper.m_Menu_DRight;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                @SelectionChange.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelectionChange;
                @SelectionChange.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelectionChange;
                @SelectionChange.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelectionChange;
                @LeftJoystick.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftJoystick;
                @LeftJoystick.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftJoystick;
                @LeftJoystick.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeftJoystick;
                @RightJoystick.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightJoystick;
                @RightJoystick.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightJoystick;
                @RightJoystick.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRightJoystick;
                @Map.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMap;
                @Map.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMap;
                @Map.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMap;
                @Crafting.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnCrafting;
                @Crafting.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnCrafting;
                @Crafting.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnCrafting;
                @Inventory.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnInventory;
                @A.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnA;
                @A.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnA;
                @A.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnA;
                @X.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnX;
                @X.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnX;
                @X.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnX;
                @B.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnB;
                @B.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnB;
                @B.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnB;
                @Y.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnY;
                @Y.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnY;
                @Y.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnY;
                @Start.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @LB.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLB;
                @LB.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLB;
                @LB.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLB;
                @RB.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRB;
                @RB.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRB;
                @RB.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRB;
                @LT.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLT;
                @LT.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLT;
                @LT.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLT;
                @RT.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnRT;
                @RT.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnRT;
                @RT.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnRT;
                @DUp.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnDUp;
                @DUp.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnDUp;
                @DUp.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnDUp;
                @DDown.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnDDown;
                @DDown.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnDDown;
                @DDown.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnDDown;
                @DLeft.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnDLeft;
                @DLeft.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnDLeft;
                @DLeft.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnDLeft;
                @DRight.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnDRight;
                @DRight.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnDRight;
                @DRight.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnDRight;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectionChange.started += instance.OnSelectionChange;
                @SelectionChange.performed += instance.OnSelectionChange;
                @SelectionChange.canceled += instance.OnSelectionChange;
                @LeftJoystick.started += instance.OnLeftJoystick;
                @LeftJoystick.performed += instance.OnLeftJoystick;
                @LeftJoystick.canceled += instance.OnLeftJoystick;
                @RightJoystick.started += instance.OnRightJoystick;
                @RightJoystick.performed += instance.OnRightJoystick;
                @RightJoystick.canceled += instance.OnRightJoystick;
                @Map.started += instance.OnMap;
                @Map.performed += instance.OnMap;
                @Map.canceled += instance.OnMap;
                @Crafting.started += instance.OnCrafting;
                @Crafting.performed += instance.OnCrafting;
                @Crafting.canceled += instance.OnCrafting;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @A.started += instance.OnA;
                @A.performed += instance.OnA;
                @A.canceled += instance.OnA;
                @X.started += instance.OnX;
                @X.performed += instance.OnX;
                @X.canceled += instance.OnX;
                @B.started += instance.OnB;
                @B.performed += instance.OnB;
                @B.canceled += instance.OnB;
                @Y.started += instance.OnY;
                @Y.performed += instance.OnY;
                @Y.canceled += instance.OnY;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @LB.started += instance.OnLB;
                @LB.performed += instance.OnLB;
                @LB.canceled += instance.OnLB;
                @RB.started += instance.OnRB;
                @RB.performed += instance.OnRB;
                @RB.canceled += instance.OnRB;
                @LT.started += instance.OnLT;
                @LT.performed += instance.OnLT;
                @LT.canceled += instance.OnLT;
                @RT.started += instance.OnRT;
                @RT.performed += instance.OnRT;
                @RT.canceled += instance.OnRT;
                @DUp.started += instance.OnDUp;
                @DUp.performed += instance.OnDUp;
                @DUp.canceled += instance.OnDUp;
                @DDown.started += instance.OnDDown;
                @DDown.performed += instance.OnDDown;
                @DDown.canceled += instance.OnDDown;
                @DLeft.started += instance.OnDLeft;
                @DLeft.performed += instance.OnDLeft;
                @DLeft.canceled += instance.OnDLeft;
                @DRight.started += instance.OnDRight;
                @DRight.performed += instance.OnDRight;
                @DRight.canceled += instance.OnDRight;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    private int m_ControlSchemeAlphaSchemeIndex = -1;
    public InputControlScheme ControlSchemeAlphaScheme
    {
        get
        {
            if (m_ControlSchemeAlphaSchemeIndex == -1) m_ControlSchemeAlphaSchemeIndex = asset.FindControlSchemeIndex("Control Scheme Alpha");
            return asset.controlSchemes[m_ControlSchemeAlphaSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnSelectionChange(InputAction.CallbackContext context);
        void OnLeftJoystick(InputAction.CallbackContext context);
        void OnRightJoystick(InputAction.CallbackContext context);
        void OnMap(InputAction.CallbackContext context);
        void OnCrafting(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnA(InputAction.CallbackContext context);
        void OnX(InputAction.CallbackContext context);
        void OnB(InputAction.CallbackContext context);
        void OnY(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnLB(InputAction.CallbackContext context);
        void OnRB(InputAction.CallbackContext context);
        void OnLT(InputAction.CallbackContext context);
        void OnRT(InputAction.CallbackContext context);
        void OnDUp(InputAction.CallbackContext context);
        void OnDDown(InputAction.CallbackContext context);
        void OnDLeft(InputAction.CallbackContext context);
        void OnDRight(InputAction.CallbackContext context);
    }
}
