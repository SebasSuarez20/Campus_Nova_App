import { Rol } from "../enums/rol-directive"

export interface session {
    firstName: string
    lastName: string,
    identification:number,
    institutionCode:number,
    rol: Rol
}