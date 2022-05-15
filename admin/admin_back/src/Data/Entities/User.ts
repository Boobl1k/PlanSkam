import {Entity, Column, PrimaryColumn, ManyToMany, JoinTable} from 'typeorm';
import {Role} from "./Role";

//некоторые столбцы из бд намеренно не перенесены, при необходимости добавить
@Entity('AspNetUsers')
export class User {
    @PrimaryColumn({type: "nvarchar"})
    Id: string;

    @Column()
    UserName: string;

    @Column()
    FavouriteTracksId: number;

    @Column()
    OwnedPlaylistsId: number;

    @ManyToMany(() => Role, (role) => role.Users)
    @JoinTable({
        name: "AspNetUserRoles",
        joinColumn: {
            name: "UserId",
            referencedColumnName: "Id"
        },
        inverseJoinColumn: {
            name: "RoleId",
            referencedColumnName: "Id"
        }
    })
    Roles: Role[]
}