export interface AuthResponseModel {
    token:string,
    expiration:Date,
    userId:string,
    email:string,
    firstname:string,
    lastname:string,
    isAdmin:boolean,
    isActive:boolean
    
}