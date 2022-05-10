import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

//некоторые столбцы из бд намеренно не перенесены, при необходимости добавить
@Entity()
export class AspNetUser {
    @PrimaryGeneratedColumn()
    Id: string;

    @Column()
    UserName: string;

    @Column()
    FavouriteTracksId: number = 0;

    @Column()
    OwnedPlaylistId: number = 0;
}