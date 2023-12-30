# VCOMM

VCOMM is a voice recognition software used to assist in commanding units/AI in video games.

VCOMM allows you to set macros to voice commands to give you that extra edge when playing your favorite games.

## Installation

Download VCOMM directly from the site [HERE](https://vcomm.publiczeus.com).

## Usage
VCOMM has a user-friendly UI that makes configuration and usage easy.
### Objects
VComm makes use of several objects/classes to configure usage.
#### VPack
A VPack is a package of VRequests, this is used to save your configurations. (used primarily to save games)
```javascript
{
    "name": "Ready Or Not",
    "author": "Devlin",
    "vRequests": [{
            "phrases": [
                "red",
                "red team"
            ],
            "macro": {
                "msToWait": 0,
                "keycodes": [
                    "{F7}"
                ]
            }
        },
        {
            "phrases": [
                "blue",
                "blue team"
            ],
            "macro": {
                "msToWait": 0,
                "keycodes": [
                    "{F6}"
                ]
            }
        }
    ]
}
```
#### VRequests
A VRequest contains the Phrases that the VComm Voice Engine will recognize and the Macro that should be executed.
```javascript
{
    "vRequests": [{
            "phrases": [
                "red",
                "red team"
            ],
            "macro": {
                "msToWait": 0,
                "keycodes": [
                    "{F7}"
                ]
            }
        },
        {
            "phrases": [
                "blue",
                "blue team"
            ],
            "macro": {
                "msToWait": 0,
                "keycodes": [
                    "{F6}"
                ]
            }
        }
    ]
}
```
#### Macro
A Macro contains the Keycodes that are executed when the attached Phrase is recognized. It also allows for a ms delay to be added. (delay is awaited after each keycode has been sent.)
```javascript
{
    "macro": {
        "msToWait": 0,
        "keycodes": [
            "{F6}"
        ]
    }
}
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)