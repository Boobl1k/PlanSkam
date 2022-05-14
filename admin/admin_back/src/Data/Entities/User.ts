import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

//некоторые столбцы из бд намеренно не перенесены, при необходимости добавить
@Entity('AspNetUsers')
export class User {
    @PrimaryGeneratedColumn()
    Id: string;

    @Column()
    UserName: string;

    @Column()
    FavouriteTracksId: number = 0;

    @Column()
    OwnedPlaylistsId: number = 0;
}