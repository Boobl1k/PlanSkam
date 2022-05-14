import {Entity, Column, PrimaryGeneratedColumn, TableForeignKey} from 'typeorm';

@Entity('Playlists')
export class Playlist {
    @PrimaryGeneratedColumn()
    Id: number;

    @Column()
    Name: string;

    @Column()
    OwnedById: number;
}