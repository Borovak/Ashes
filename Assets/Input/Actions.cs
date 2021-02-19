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
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""c26e9aaf-70a1-4051-9ccb-b7ea3eff8887"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
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
                    ""name"": ""AttackSpell"",
                    ""type"": ""Button"",
                    ""id"": ""92f1c34b-6fc8-42ba-b240-77d5c0fbbd0d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SelfSpell"",
                    ""type"": ""Button"",
                    ""id"": ""36e0cb63-3bee-42b5-96a0-d935ab6cfe26"",
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
                },
                {
                    ""name"": ""Shield"",
                    ""type"": ""Button"",
                    ""id"": ""34143677-6945-4869-a85d-6d0aa2872df6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
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
                    ""id"": ""4761bc3f-6bd5-44ce-92b5-98a512357ea0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8852de19-d0f5-4fe6-8125-5c7961b14725"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0dadef5c-d3a9-41fe-a13d-1587e6b68be0"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Attack"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""ae775212-d1ae-49f1-a66c-598a3c64d9bb"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""SelfSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81f02893-9718-4033-a7a2-7b8e6ea827d2"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""SelfSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""319fe034-4fa5-4514-bacc-85c5fd5c932d"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""AttackSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a3c0b58-9d86-42b9-86ae-712a9f778a24"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""AttackSpell"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""548618ea-927f-496b-9e49-8b617b7921ed"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Shield"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79f9bb0d-aa25-4221-8797-4e1c0437c8eb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Shield"",
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
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0f49247f-0d31-41af-a537-e7b4dfdc6ff6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OK"",
                    ""type"": ""Button"",
                    ""id"": ""a2f7b40b-4c5d-46ce-a32d-c8c855dc2a8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""0a1456cc-ad1c-47b8-9756-cf26ae0464a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""d7b16516-bd5e-4777-968d-d1797892724d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""a36ab08a-a456-49c4-9d52-e879d0198f7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SectionPrevious"",
                    ""type"": ""Button"",
                    ""id"": ""73135f96-fb79-4869-9465-be820cb57f3a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""SectionNext"",
                    ""type"": ""Button"",
                    ""id"": ""f825c761-529f-4089-891c-f4cae751cb7d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MapZoomIn"",
                    ""type"": ""Button"",
                    ""id"": ""d2afb073-b029-40c7-a331-ace487d429ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MapZoomOut"",
                    ""type"": ""Button"",
                    ""id"": ""c233f7c5-bfaf-460c-9f45-6e445b1fbdb6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
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
                    ""name"": ""Special"",
                    ""type"": ""Button"",
                    ""id"": ""06a9b880-2a1f-4103-92c6-74ab2edc2ddf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
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
                    ""name"": """",
                    ""id"": ""62c0fa14-9093-46de-a930-2627ce6e28e9"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""OK"",
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
                    ""action"": ""OK"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c44926d6-1a93-4b63-9212-2d8a3beee978"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Back"",
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
                    ""action"": ""Back"",
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
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Start"",
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
                    ""id"": ""cb32f62b-bc00-4ea8-b782-a6c1705ac5f5"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""SectionPrevious"",
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
                    ""action"": ""SectionPrevious"",
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
                    ""action"": ""SectionNext"",
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
                    ""action"": ""SectionNext"",
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
                    ""action"": ""MapZoomIn"",
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
                    ""action"": ""MapZoomIn"",
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
                    ""action"": ""MapZoomOut"",
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
                    ""action"": ""MapZoomOut"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""b6fe5f39-6f3e-48a4-84c2-10a0d829aba1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""id"": ""2713365a-ab23-4b35-ad26-8b952ac5c5cd"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Control Scheme Alpha"",
                    ""action"": ""Special"",
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
                    ""action"": ""Special"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_AttackSpell = m_Player.FindAction("AttackSpell", throwIfNotFound: true);
        m_Player_SelfSpell = m_Player.FindAction("SelfSpell", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Shield = m_Player.FindAction("Shield", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_SelectionChange = m_Menu.FindAction("Selection Change", throwIfNotFound: true);
        m_Menu_Movement = m_Menu.FindAction("Movement", throwIfNotFound: true);
        m_Menu_OK = m_Menu.FindAction("OK", throwIfNotFound: true);
        m_Menu_Back = m_Menu.FindAction("Back", throwIfNotFound: true);
        m_Menu_Start = m_Menu.FindAction("Start", throwIfNotFound: true);
        m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
        m_Menu_SectionPrevious = m_Menu.FindAction("SectionPrevious", throwIfNotFound: true);
        m_Menu_SectionNext = m_Menu.FindAction("SectionNext", throwIfNotFound: true);
        m_Menu_MapZoomIn = m_Menu.FindAction("MapZoomIn", throwIfNotFound: true);
        m_Menu_MapZoomOut = m_Menu.FindAction("MapZoomOut", throwIfNotFound: true);
        m_Menu_Map = m_Menu.FindAction("Map", throwIfNotFound: true);
        m_Menu_Crafting = m_Menu.FindAction("Crafting", throwIfNotFound: true);
        m_Menu_Special = m_Menu.FindAction("Special", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_AttackSpell;
    private readonly InputAction m_Player_SelfSpell;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Shield;
    public struct PlayerActions
    {
        private @Actions m_Wrapper;
        public PlayerActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @AttackSpell => m_Wrapper.m_Player_AttackSpell;
        public InputAction @SelfSpell => m_Wrapper.m_Player_SelfSpell;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Shield => m_Wrapper.m_Player_Shield;
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
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @AttackSpell.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttackSpell;
                @AttackSpell.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttackSpell;
                @AttackSpell.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttackSpell;
                @SelfSpell.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelfSpell;
                @SelfSpell.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelfSpell;
                @SelfSpell.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelfSpell;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Shield.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShield;
                @Shield.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShield;
                @Shield.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShield;
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
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @AttackSpell.started += instance.OnAttackSpell;
                @AttackSpell.performed += instance.OnAttackSpell;
                @AttackSpell.canceled += instance.OnAttackSpell;
                @SelfSpell.started += instance.OnSelfSpell;
                @SelfSpell.performed += instance.OnSelfSpell;
                @SelfSpell.canceled += instance.OnSelfSpell;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Shield.started += instance.OnShield;
                @Shield.performed += instance.OnShield;
                @Shield.canceled += instance.OnShield;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_SelectionChange;
    private readonly InputAction m_Menu_Movement;
    private readonly InputAction m_Menu_OK;
    private readonly InputAction m_Menu_Back;
    private readonly InputAction m_Menu_Start;
    private readonly InputAction m_Menu_Select;
    private readonly InputAction m_Menu_SectionPrevious;
    private readonly InputAction m_Menu_SectionNext;
    private readonly InputAction m_Menu_MapZoomIn;
    private readonly InputAction m_Menu_MapZoomOut;
    private readonly InputAction m_Menu_Map;
    private readonly InputAction m_Menu_Crafting;
    private readonly InputAction m_Menu_Special;
    public struct MenuActions
    {
        private @Actions m_Wrapper;
        public MenuActions(@Actions wrapper) { m_Wrapper = wrapper; }
        public InputAction @SelectionChange => m_Wrapper.m_Menu_SelectionChange;
        public InputAction @Movement => m_Wrapper.m_Menu_Movement;
        public InputAction @OK => m_Wrapper.m_Menu_OK;
        public InputAction @Back => m_Wrapper.m_Menu_Back;
        public InputAction @Start => m_Wrapper.m_Menu_Start;
        public InputAction @Select => m_Wrapper.m_Menu_Select;
        public InputAction @SectionPrevious => m_Wrapper.m_Menu_SectionPrevious;
        public InputAction @SectionNext => m_Wrapper.m_Menu_SectionNext;
        public InputAction @MapZoomIn => m_Wrapper.m_Menu_MapZoomIn;
        public InputAction @MapZoomOut => m_Wrapper.m_Menu_MapZoomOut;
        public InputAction @Map => m_Wrapper.m_Menu_Map;
        public InputAction @Crafting => m_Wrapper.m_Menu_Crafting;
        public InputAction @Special => m_Wrapper.m_Menu_Special;
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
                @Movement.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMovement;
                @OK.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnOK;
                @OK.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnOK;
                @OK.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnOK;
                @Back.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                @Start.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnStart;
                @Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                @SectionPrevious.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSectionPrevious;
                @SectionPrevious.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSectionPrevious;
                @SectionPrevious.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSectionPrevious;
                @SectionNext.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSectionNext;
                @SectionNext.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSectionNext;
                @SectionNext.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSectionNext;
                @MapZoomIn.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMapZoomIn;
                @MapZoomIn.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMapZoomIn;
                @MapZoomIn.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMapZoomIn;
                @MapZoomOut.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMapZoomOut;
                @MapZoomOut.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMapZoomOut;
                @MapZoomOut.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMapZoomOut;
                @Map.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMap;
                @Map.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMap;
                @Map.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMap;
                @Crafting.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnCrafting;
                @Crafting.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnCrafting;
                @Crafting.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnCrafting;
                @Special.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSpecial;
                @Special.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSpecial;
                @Special.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSpecial;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SelectionChange.started += instance.OnSelectionChange;
                @SelectionChange.performed += instance.OnSelectionChange;
                @SelectionChange.canceled += instance.OnSelectionChange;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @OK.started += instance.OnOK;
                @OK.performed += instance.OnOK;
                @OK.canceled += instance.OnOK;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @SectionPrevious.started += instance.OnSectionPrevious;
                @SectionPrevious.performed += instance.OnSectionPrevious;
                @SectionPrevious.canceled += instance.OnSectionPrevious;
                @SectionNext.started += instance.OnSectionNext;
                @SectionNext.performed += instance.OnSectionNext;
                @SectionNext.canceled += instance.OnSectionNext;
                @MapZoomIn.started += instance.OnMapZoomIn;
                @MapZoomIn.performed += instance.OnMapZoomIn;
                @MapZoomIn.canceled += instance.OnMapZoomIn;
                @MapZoomOut.started += instance.OnMapZoomOut;
                @MapZoomOut.performed += instance.OnMapZoomOut;
                @MapZoomOut.canceled += instance.OnMapZoomOut;
                @Map.started += instance.OnMap;
                @Map.performed += instance.OnMap;
                @Map.canceled += instance.OnMap;
                @Crafting.started += instance.OnCrafting;
                @Crafting.performed += instance.OnCrafting;
                @Crafting.canceled += instance.OnCrafting;
                @Special.started += instance.OnSpecial;
                @Special.performed += instance.OnSpecial;
                @Special.canceled += instance.OnSpecial;
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
        void OnAttack(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnAttackSpell(InputAction.CallbackContext context);
        void OnSelfSpell(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnShield(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnSelectionChange(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnOK(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnSectionPrevious(InputAction.CallbackContext context);
        void OnSectionNext(InputAction.CallbackContext context);
        void OnMapZoomIn(InputAction.CallbackContext context);
        void OnMapZoomOut(InputAction.CallbackContext context);
        void OnMap(InputAction.CallbackContext context);
        void OnCrafting(InputAction.CallbackContext context);
        void OnSpecial(InputAction.CallbackContext context);
    }
}
