import {Entity, Column, PrimaryGeneratedColumn} from 'typeorm';

@Entity('PlaylistUser')
export class PlaylistUser {
    @PrimaryGeneratedColumn()
    PlaylistId: number;

    @Column()
    UserId: string;
}